using Flat4Me.Core.Auth;
using Flat4Me.Data.DTO.Short;
using Flat4Me.Data.Repository.Interfaces.Short;
using Flat4Me.Identity;
using Flat4Me.Web.Portal.App_Start;
using Flat4Me.Web.Portal.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Ninject;
using System;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Flat4Me.Web.Portal.Controllers
{
    [F4MeAuthorize()]
    public class AccountController : BaseController
    {
        [Inject]
        public IHotelierProfileRepository HotelierProfileRepository { get; set; }

        /// <summary>
        /// Check user claim fully. Check in db.
        /// </summary>
        /// <param name="claim">Claim to check</param>
        /// <param name="checkInDb">Input params is UserId. Output is boolean result</param>
        /// <returns></returns>
        private async Task<bool> CheckClaim(Claim claim, Func<int, Task<bool>> checkInDb)
        {
            // Cached user identity
            var userIdentity = User.Identity as ClaimsIdentity;

            var hasCachedClaim = userIdentity.HasClaim(claim.Type, claim.Value);
            if (hasCachedClaim)
                return true;

            // Confirmed in db, but claim is not given. In case when login from different browser
            if (await checkInDb(UserId))
            {
                var dbClaims = await UserManager.GetClaimsAsync(UserId);
                var hasClaimInDb = dbClaims.Any(p => p.Type == claim.Type && p.Value == claim.Value);
                if (hasClaimInDb == false)
                {
                    await UserManager.AddClaimAsync(UserId, claim);
                }

                await RefreshCurrentUserCookie();

                return true;
            }
            return false;// RedirectToAction("EmailConfirmation");
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public async Task<ActionResult> Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                // Cached user identity
                var userIdentity = User.Identity as ClaimsIdentity;

                // Check email confirmed
                var emailConfirmedClaim = F4MeClaims.GetEmailConfirmed();
                if (await CheckClaim(emailConfirmedClaim, UserManager.IsEmailConfirmedAsync) == false)
                {
                    return RedirectToAction("EmailConfirmation");
                }

                //var emailConfirmed = userIdentity.HasClaim(emailConfirmedClaim.Type, emailConfirmedClaim.Value);
                //if (emailConfirmed == false)
                //{
                //    // Confirmed in db, but claim is not given. In case when login from different browser
                //    if (await UserManager.IsEmailConfirmedAsync(UserId))
                //    {
                //        var dbClaims = await UserManager.GetClaimsAsync(UserId);
                //        var hasClaimInDb = dbClaims.Any(p => p.Type == emailConfirmedClaim.Type && p.Value == emailConfirmedClaim.Value);
                //        if (hasClaimInDb == false)
                //        {
                //            await UserManager.AddClaimAsync(UserId, emailConfirmedClaim);
                //        }

                //        await RefreshCurrentUserCookie();

                //        return RedirectToLocal(returnUrl);
                //    }

                //    return RedirectToAction("EmailConfirmation");
                //}

                // Check phone confirmed
                var phoneConfirmedClaim = F4MeClaims.GetPhoneConfirmed();
                if (await CheckClaim(phoneConfirmedClaim, UserManager.IsPhoneNumberConfirmedAsync) == false)
                {
                    return RedirectToAction("PhoneConfirmation");
                }
                //var phoneConfirmed = userIdentity.HasClaim(phoneConfirmedClaim.Type, phoneConfirmedClaim.Value);
                //if (phoneConfirmed == false)
                //{
                //    // Confirmed in db, but claim is not given. In case when login from different browser
                //    if (await UserManager.IsPhoneNumberConfirmedAsync(UserId))
                //    {
                //        var dbClaims = await UserManager.GetClaimsAsync(UserId);
                //        var hasClaimInDb = dbClaims.Any(p => p.Type == phoneConfirmedClaim.Type && p.Value == phoneConfirmedClaim.Value);
                //        if (hasClaimInDb == false)
                //        {
                //            await UserManager.AddClaimAsync(UserId, phoneConfirmedClaim);
                //        }

                //        await RefreshCurrentUserCookie();

                //        return RedirectToLocal(returnUrl);
                //    }

                //    return RedirectToAction("PhoneConfirmation");
                //}

                // Check role
                if (User.IsInRole(UserRoleList.Hotelier) == false)
                {
                    // User has role, but it is not present in cookie. In case when login from different browser
                    if (UserManager.IsInRole(UserId, UserRoleList.Hotelier))
                    {
                        await RefreshCurrentUserCookie();

                        return RedirectToLocal(returnUrl);
                    }

                    return RedirectToAction("HotelierRegistration");
                }

                // Check hotelier approved
                var hotelierApprovedClaim = F4MeClaims.GetHotelierApproved();
                if (await CheckClaim(hotelierApprovedClaim,
                    async userId =>
                    {
                        var hotelierProfile = await HotelierProfileRepository.Get(userId);
                        return hotelierProfile != null && hotelierProfile.IsApproved;
                    }) == false)
                {
                    return RedirectToAction("HotelierIsNotApproved");
                }

                //var hotelierApproved = userIdentity.HasClaim(hotelierApprovedClaim.Type, hotelierApprovedClaim.Value);
                //if (hotelierApproved == false)
                //{
                //    var hotelierProfile = await HotelierProfileRepository.Get(UserId);
                //    // User has profile and it is approved. In case when login from different browser
                //    if (hotelierProfile != null && hotelierProfile.IsApproved)
                //    {
                //        var dbClaims = await UserManager.GetClaimsAsync(UserId);
                //        var hasClaimInDb = dbClaims.Any(p => p.Type == hotelierApprovedClaim.Type && p.Value == hotelierApprovedClaim.Value);
                //        if (hasClaimInDb == false)
                //        {
                //            await UserManager.AddClaimAsync(UserId, hotelierApprovedClaim);
                //        }

                //        await RefreshCurrentUserCookie();

                //        return RedirectToLocal(returnUrl);
                //    }
                //    return RedirectToAction("HotelierIsNotApproved");
                //}
                                
                if (string.IsNullOrEmpty(returnUrl)==false
                    && returnUrl.ToLower().Contains("login")==false)
                {
                    // In this case we update user's cached claims and roles and redirect to desired action
                    return RedirectToLocal(returnUrl);
                }
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: true);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }


        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new F4MeUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: true, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        /// <summary>
        /// Prepare and send email confirmation url to user
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> EmailConfirmation()
        {
            await GenerateAndSendEmailConfirmation();
            return View();
        }
        /// <summary>
        /// Call by user by email url for email confirmation
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<ActionResult> ConfirmEmail(string code)
        {
            if (code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(UserId, code);
            if (result.Succeeded)
            {
                var claim = F4MeClaims.GetEmailConfirmed();
                await UserManager.AddClaimAsync(UserId, claim);
                await RefreshCurrentUserCookie();
            }
            else
            {
                AddErrors(result);
            }

            return RedirectToAction("Login");
        }


        public async Task<ActionResult> PhoneConfirmation()
        {
            var user = await CurrentUser();
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(UserId, user.PhoneNumber);
            //await UserManager.SendSmsAsync(UserId, code);

            return View(new PhoneConfirmationViewModel { Code = code });
        }
        [HttpPost]
        public async Task<ActionResult> PhoneConfirmation(PhoneConfirmationViewModel model)
        {
            var user = await CurrentUser();
            var result = await UserManager.ChangePhoneNumberAsync(UserId, user.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var claim = F4MeClaims.GetPhoneConfirmed();
                await UserManager.AddClaimAsync(UserId, claim);
                await RefreshCurrentUserCookie();
            }
            else
            {
                AddErrors(result);
            }
            // Anyway redirect to Login action.
            // If code success then Login perform next login actions
            // If code wrong then Login perform new code generation action
            return RedirectToAction("Login");
        }


        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Восстановление пароля", "Для восстановления пароля перейдите по <a href=\"" + callbackUrl + "\">ссылке</a>");
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }
        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }
            AddErrors(result);
            return View();
        }
        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }
        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: true);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }
        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new F4MeUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.UserId, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: true, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }


        public ActionResult HotelierRegistration()
        {
            return View(new HotelierRegistrationViewModel
            {
                CheckinFrom = new TimeSpan(13, 0, 0),
                CheckoutTo = new TimeSpan(11, 0, 0),
                HasAirportTransfer = false,
                HasTrainTransfer = false
            });
        }
        [HttpPost]
        public async Task<ActionResult> HotelierRegistration(HotelierRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dto = new HotelierProfileDTO
                {
                    UserId = UserId,
                    CityId = model.CityId.Value,
                    CheckinFrom = model.CheckinFrom.Value,
                    CheckinTo = model.CheckinTo,
                    CheckoutFrom = model.CheckoutFrom,
                    CheckoutTo = model.CheckoutTo.Value,
                    HasAirportTransfer = model.HasAirportTransfer,
                    EstimatedAirportTransferCost = model.HasAirportTransfer ? model.EstimatedAirportTransferCost.Value : new int?(),
                    HasTrainTransfer = model.HasTrainTransfer,
                    EstimatedTrainTransferCost = model.HasTrainTransfer ? model.EstimatedTrainTransferCost.Value : new int?(),
                };

                await HotelierProfileRepository.Add(dto);
                await UserManager.AddToRoleAsync(UserId, UserRoleList.Hotelier);
                await NotifyHotelierAboutProfileCreated();
                await NotifyAdminsAboutNewHotelier();
                await RefreshCurrentUserCookie();

                // Anyway redirect to Login action.
                // If code success then Login perform next login actions
                // If code wrong then Login perform new code generation action
                return RedirectToAction("Login");
            }
            return View(model);
        }


        public async Task<ActionResult> HotelierIsNotApproved()
        {
            var hotelierProfile = await HotelierProfileRepository.Get(UserId);
            if (hotelierProfile.IsApproved)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

        private async Task GenerateAndSendEmailConfirmation()
        {
            var code = await UserManager.GenerateEmailConfirmationTokenAsync(UserId);
            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Account",
                new { code = code },
                protocol: Request.Url.Scheme);

            var subject = "Подтверждение аккаунта";
            var body = new StringBuilder();
            body.AppendFormat(@"Пожалуйста, подтвердите ваш аккаунт нажав на <a href='{0}'>ссылку</a>", callbackUrl);
            body.Append("<br /><br />");
            body.Append("Спасибо!");

            await UserManager.SendEmailAsync(
                UserId,
                subject,
                body.ToString());
        }
        private async Task NotifyHotelierAboutProfileCreated()
        {
            var subject = "Аккаунт Хотельера успешно создан";
            var body = new StringBuilder();
            body.Append("Вы успешно зарегестрированы в системе как Хотельер");
            body.Append("<br /><br />");
            body.Append("Осталось еще чуть-чуть. Наши модераторы уже проверяют ваши данные!");
            body.Append("<br /><br />");
            body.Append("Вы получите уведомление сразу, как только аккаунт будет подтвержден!");
            body.Append("<br /><br />");
            body.Append("Спасибо!");

            await UserManager.SendEmailAsync(
                UserId,
                subject,
                body.ToString());
        }
        private async Task NotifyAdminsAboutNewHotelier()
        {
            if (UserManager.EmailService == null)
                throw new NullReferenceException("NotifyAdminsAbountNewHotelier => EmailService");

            var adminEmailList = ConfigurationManager.AppSettings["AdminEmailList"];
            if (string.IsNullOrEmpty(adminEmailList))
                throw new NullReferenceException("NotifyAdminsAbountNewHotelier => AdminEmailList");

            var user = await UserManager.FindByIdAsync(UserId);

            var subject = string.Format("Внимание! Новый Хотельер! {0} {1}", user.LastName, user.FirstName);
            var body = new StringBuilder();
            body.Append("<h3>В системе зарегистрировался новый Хотельер</h3>");
            body.Append("<br/>");
            body.Append("Необходимо перейти по <a href=\"\">ссылке</a> и подтвердить аккаунт, чтобы пользователь мог начать сдавать жилье");
            body.Append("<br/><br/>");
            body.Append("Данные Хотельера:");
            body.Append("<br/>");
            body.AppendFormat("{0} {1}", user.LastName, user.FirstName);
            body.Append("<br/>");
            body.AppendFormat(@"<img src='{0}' />", user.PhotoSmallPath);
            body.Append("<br/>");
            body.AppendFormat("Email:    {0}", user.Email);
            body.Append("<br/>");
            body.AppendFormat("Телефон:  {0}", user.PhoneNumber);

            var msg = new IdentityMessage
            {
                Destination = adminEmailList,
                Subject = subject,
                Body = body.ToString()
            };

            await UserManager.EmailService.SendAsync(msg);
        }
        private async Task RefreshCurrentUserCookie()
        {
            await SignInManager.SignInAsync(await CurrentUser(), true, false);
        }
    }
}