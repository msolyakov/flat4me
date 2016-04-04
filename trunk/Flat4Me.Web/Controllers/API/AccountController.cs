using Flat4Me.Core.Auth;
using Flat4Me.Identity;
using Flat4Me.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Flat4Me.Web.Controllers.Api
{
    [F4MeWebApiAuthorize]
    [RoutePrefix("api/account")]
    public class AccountController : BaseApiController
    {
        private readonly int MIN_SECONDS_BEFORE_SEND_NEW_CODE = 30;

        [HttpGet]
        [Route("info")]
        public async Task<IHttpActionResult> GetInfo()
        {
            var u = await UserManager.FindByIdAsync(UserId);
            var user = new UserModel
            {
                LastName = u.LastName,
                FirstName = u.FirstName,
                PhoneNumber = u.PhoneNumber,
                PhoneNumberConfirmed = u.PhoneNumberConfirmed,
            };

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("registerGuest")]
        public async Task<IHttpActionResult> RegisterGuest(GuestRegistrationModel guest)
        {
            if (guest == null
                || string.IsNullOrEmpty(guest.Email)
                || string.IsNullOrEmpty(guest.FirstName)
                || string.IsNullOrEmpty(guest.LastName)
                || string.IsNullOrEmpty(guest.PhoneNumber))
            {
                return BadRequest();
            }

            var identity = await UserManager.FindByEmailAsync(guest.Email);
            // There is no user with identically email
            if (identity == null)
            {
                // Register new user
                identity = new F4MeUser
                {
                    FirstName = guest.FirstName,
                    LastName = guest.LastName,
                    PhoneNumber = guest.PhoneNumber,
                    Email = guest.Email
                };

                var identityResult = await UserManager.CreateAsync(identity);
                if (identityResult.Succeeded == false)
                {
                    return BadRequest();
                }

                await UserManager.AddToRoleAsync(identity.UserId, UserRoleList.Guest);
            }
            // User already exist
            else
            {
                var userLogins = await UserManager.GetLoginsAsync(identity.UserId);

                // User has password or external login. Need LoginIn
                if (string.IsNullOrEmpty(identity.PasswordHash) == false
                    || userLogins.Count > 0)
                {
                    // TODO
                    return BadRequest();
                    //return ResponseMessage(new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized));
                    //return System.Web.Http.Results.
                }

                // In this case user registred without password or some external login. Simply signin this user below
            }

            await SignInManager.SignInAsync(identity, isPersistent: true, rememberBrowser: true);

            if (identity.PhoneNumberConfirmed == false)
                if (await CheckLastConfirmSmsDateTimeIsOk(identity))
                    await SendConfirmationCode(identity);

            var userModel = new UserModel
            {
                LastName = identity.LastName,
                FirstName = identity.FirstName,
                PhoneNumber = identity.PhoneNumber,
                PhoneNumberConfirmed = identity.PhoneNumberConfirmed
            };

            return Ok(userModel);
        }

        [HttpPut]
        [Route("confirmPhoneNumber/{code?}")]
        public async Task<IHttpActionResult> ConfirmPhoneNumber(string code)
        {
            var user = await CurrentUser();
            var result = await UserManager.ChangePhoneNumberAsync(UserId, user.PhoneNumber, code);
            if (result.Succeeded)
            {
                var claims = await UserManager.GetClaimsAsync(UserId);
                var claim = F4MeClaims.GetPhoneConfirmed();
                // Has no PhoneConfirmed claim
                if (claims.Any(p => F4MeClaims.Compare(p, claim)) == false)
                {
                    await UserManager.AddClaimAsync(UserId, claim);
                }

                // Remove unused claims
                foreach (var item in claims.Where(p => p.Type == F4MeClaims.LAST_CONFIRM_SMS_DATETIME))
                {
                    await UserManager.RemoveClaimAsync(UserId, item);
                }

                var identity = await UserManager.FindByIdAsync(UserId);
                await SignInManager.SignInAsync(identity, isPersistent: true, rememberBrowser: true);
            }

            return Ok(result.Succeeded);
        }

        [HttpGet]
        [Route("requestConfirmationCode")]
        public async Task<IHttpActionResult> RequestConfirmationCode()
        {
            var identity = await UserManager.FindByIdAsync(UserId);
            if (identity == null)
                return Ok(false);

            if (await CheckLastConfirmSmsDateTimeIsOk(identity) == false)
                return Ok(false);

            await SendConfirmationCode(identity);

            return Ok(true);
        }

        /// <summary>
        /// Проверяет достаточно ли прошло времени после последней отправки смс. По умолчанию 300 сек
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CheckLastConfirmSmsDateTimeIsOk(F4MeUser identity)
        {
            // Check last send time
            var claimList = await UserManager.GetClaimsAsync(identity.UserId);
            // Здесь может быть несколько клаймов, нам нужен самый старший
            var lastSendDateTimeClaim = claimList
                .Where(p => p.Type == F4MeClaims.LAST_CONFIRM_SMS_DATETIME)
                .OrderByDescending(p => Convert.ToInt64(p.Value))
                .FirstOrDefault();
            if (lastSendDateTimeClaim != null)
            {
                var ticks = Convert.ToInt64(lastSendDateTimeClaim.Value);
                var lastSendDateTime = new DateTime(ticks);
                var diff = DateTime.UtcNow - lastSendDateTime;
                // Прошло не достаточно времени, прежде чем отправлять новый код
                if (diff.TotalSeconds < MIN_SECONDS_BEFORE_SEND_NEW_CODE)
                {
                    return await Task.FromResult<bool>(false);
                }
                // 300 секунд прошло, удаляем ненужный клайм
                else
                {
                    // Удаляем все побочные клаймы.
                    foreach (var claim in claimList.Where(p => p.Type == F4MeClaims.LAST_CONFIRM_SMS_DATETIME))
                    {
                        await UserManager.RemoveClaimAsync(identity.UserId, claim);
                    }
                }
            }

            return await Task.FromResult<bool>(true);
        }

        /// <summary>
        /// Отправить пользователю смс для подтверждения телефона
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        private async Task SendConfirmationCode(F4MeUser identity)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(identity.UserId, identity.PhoneNumber);
            var sms = string.Format("Flat4.me код: {0}", code);

            await UserManager.SendSmsAsync(identity.UserId, sms);

            var lastSendDateTimeClaim = F4MeClaims.GetLAST_CONFIRM_SMS_DATETIME(DateTime.UtcNow.Ticks.ToString());
            await UserManager.AddClaimAsync(identity.UserId, lastSendDateTimeClaim);
        }
    }
}