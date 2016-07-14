using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Flat4Me.Identity
{
    /// <summary>
    ///     Validates users before they are saved to an IUserStore
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class F4MeUserValidator<TUser> : UserValidator<TUser, int>
        where TUser : F4MeUser
    {
        public F4MeUserValidator(UserManager<TUser, int> manager)
            : base(manager)
        {
            this.Manager = manager as F4MeUserManager;
        }

        private F4MeUserManager Manager { get; set; }

        /// <summary>
        /// Validates a user before saving
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override async Task<IdentityResult> ValidateAsync(TUser item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            var errors = new List<string>();

            await ValidateEmail(item, errors);
            await ValidatePhoneNumber(item, errors);

            if (errors.Count > 0)
                return IdentityResult.Failed(errors.ToArray());

            return IdentityResult.Success;
        }

        private Task ValidatePhoneNumber(TUser user, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(user.PhoneNumber))
            {
                errors.Add("Телефонный номер обязательное поле");
                return Task.FromResult<object>(null);
            }

            if (!Regex.IsMatch(user.PhoneNumber, @"^[0-9]{10}$"))
            {
                // If any characters are not letters or digits, its an illegal user name
                errors.Add("Телефон введен не верно. Верный формат 9876543210. 10 цифр, без 8 и +7, без пробелов и скобок");
                return Task.FromResult<object>(null);
            }

            return Task.FromResult<object>(null);
        }

        // make sure email is not empty, valid, and unique
        private async Task ValidateEmail(TUser user, List<string> errors)
        {
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrWhiteSpace(user.Email))
            {
                errors.Add("Email пустой. Это поле обязательно");
                return;
            }
            try
            {
                var m = new MailAddress(user.Email);
            }
            catch (FormatException)
            {
                errors.Add("Email не верный. Пожалуйста, проверьте");
                return;
            }

            var owner = await Manager.FindByEmailAsync(user.Email);
            if (owner != null && !EqualityComparer<int>.Default.Equals(owner.Id, user.Id))
                errors.Add("Email уже используется другим пользователем. Вы уверены, что это ваш адрес?");
        }
    }
}
