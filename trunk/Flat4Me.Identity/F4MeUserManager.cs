using Flat4Me.Data.DTO.Auth;
using Flat4Me.Identity.Identity;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.Identity
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class F4MeUserManager : UserManager<F4MeUser, int>
    {
        private F4MeUserStore UserStore { get; set; }


        public F4MeUserManager(F4MeUserStore store)
            : base(store)
        {
            UserStore = store;
        }

        /// <summary>
        /// Add phone number to Auth_UserPhone table.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public Task AddPhoneNumber(int userId, string phone)
        {
            return UserStore.AddPhoneNumber(userId, phone);
        }

        /// <summary>
        /// Remove user phone from Auth_UserPhone table
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public Task RemovePhoneNumber(int userId, string phone)
        {
            return UserStore.RemovePhoneNumber(userId, phone);
        }

        /// <summary>
        /// Get use phone numbers from Auth_UserPhone table.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<IEnumerable<UserPhoneDTO>> GetPhoneNumberListAsync(int userId)
        {
            return UserStore.GetPhoneNumberList(userId);
        }


        /// <summary>
        /// Attention!!! Gegerate confirmation code for phone from Auth_UserPhone database table.
        /// For phone from Auth_User table use standart method GenerateChangePhoneNumberTokenAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public async Task<string> GenerateUserPhoneNumberConfirmationCode(int userId, string phone)
        {
            var stamp = await GetSecurityStampAsync(userId);

            var token = new SecurityToken(Encoding.Unicode.GetBytes(stamp));

            var num = Rfc6238AuthenticationService.GenerateCode(token, phone);

            return await Task.FromResult<string>(num.ToString("D6", CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Attention!!! Confirmation for phone from Auth_UserPhone database table.
        /// For phone from Auth_User table use standart method ChangePhoneNumberAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<bool> ConfirmUserPhoneNumber(int userId, string phone, string code)
        {
            int num;
            if (int.TryParse(code, out num) == false)
            {
                return await Task.FromResult<bool>(false);
            }

            var stamp = await this.GetSecurityStampAsync(userId);

            var token = new SecurityToken(Encoding.Unicode.GetBytes(stamp));

            await this.UpdateSecurityStampAsync(userId);

            var valid = Rfc6238AuthenticationService.ValidateCode(token, num, phone);
            if (valid)
            {
                await UserStore.SetPhoneNumberConfirmed(userId, phone, true);
            }

            return await Task.FromResult<bool>(valid);
        }

        public async override Task SendSmsAsync(int userId, string message)
        {
            if (SmsService != null)
            {
                var identityMessage = new IdentityMessage();
                var phoneNumber = await GetPhoneNumberAsync(userId);
                identityMessage.Destination = "+7" + phoneNumber;
                identityMessage.Body = message;
                await SmsService.SendAsync(identityMessage);
            }
            else
            {
                await base.SendSmsAsync(userId, message);
            }
        }
    }
}
