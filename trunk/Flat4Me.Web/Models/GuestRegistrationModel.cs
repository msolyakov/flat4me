using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Flat4Me.Web.Models
{
    public class GuestRegistrationModel : IValidatableObject
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            #region Validate simple required fields

            if (string.IsNullOrEmpty(FirstName))
                result.Add(new ValidationResult("Обязательно заполните", new[] { "FirstName" }));

            if (string.IsNullOrEmpty(LastName))
                result.Add(new ValidationResult("Обязательно заполните", new[] { "LastName" }));

            if (string.IsNullOrEmpty(PhoneNumber))
                result.Add(new ValidationResult("Обязательно заполните", new[] { "PhoneNumber" }));

            if (string.IsNullOrEmpty(Email))
                result.Add(new ValidationResult("Обязательно заполните", new[] { "Email" }));

            #endregion

            result.AddRange(FirstLastName_CheckAndFix());
            result.AddRange(PhoneNumber_CheckAndFix());
            result.AddRange(Email_CheckAndFix());

            return result;
        }

        private IEnumerable<ValidationResult> FirstLastName_CheckAndFix()
        {
            var result = new List<ValidationResult>();

            FirstName = FirstName.Replace(" ", "");
            LastName = LastName.Replace(" ", "");
            // Сделать с заглавной буквы саша => Саша
            FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(FirstName);
            LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(LastName);

            // Имя и Фамилия в одну букву не канают
            if (FirstName.Length == 1)
                result.Add(new ValidationResult("Введите имя правильно", new[] { "FirstName" }));

            if (LastName.Length == 1)
                result.Add(new ValidationResult("Введите фамилию правильно", new[] { "LastName" }));

            return result;
        }
        private IEnumerable<ValidationResult> PhoneNumber_CheckAndFix()
        {
            var result = new List<ValidationResult>();

            PhoneNumber = PhoneNumber.Replace(" ", "");
            PhoneNumber = PhoneNumber.Replace("(", "");
            PhoneNumber = PhoneNumber.Replace(")", "");
            PhoneNumber = PhoneNumber.Replace("+", "");
            PhoneNumber = PhoneNumber.Replace(".", "");
            PhoneNumber = PhoneNumber.Replace("-", "");

            var validator = new PhoneAttribute();

            // Validate and fix PhoneNumber
            // 9277607222 - 10 цифр, правильный
            // Проверяем и по возможности исправляем ошибки пользователя.
            if (PhoneNumber.Length == 10)
            {
                var phone = 0L;
                var phoneIsCorrect = long.TryParse(PhoneNumber, out phone);
                if (phoneIsCorrect == false)
                {
                    result.Add(new ValidationResult("Введите номер правильно", new[] { "PhoneNumber" }));
                }
            }
            else if (PhoneNumber.Length > 10)
            {
                var firstNum = PhoneNumber[0];
                if (firstNum == '8' || firstNum == '7')
                {
                    PhoneNumber = PhoneNumber.Remove(0, 1);
                    return PhoneNumber_CheckAndFix();
                }
                else
                {
                    var valid = validator.IsValid(PhoneNumber);
                    if (valid)
                    {
                        result.Add(new ValidationResult("Проверьте свой телефон, должно быть 10 цифр", new[] { "PhoneNumber" }));
                    }
                }
            }
            else
            {
                result.Add(new ValidationResult("Введите номер правильно", new[] { "PhoneNumber" }));
            }

            // Если ручная проверка ни чего не нашла, проверяем автоматической
            if (result.Count == 0)
            {
                var valid = validator.IsValid(PhoneNumber);
                if (valid == false)
                {
                    result.Add(new ValidationResult("Введите номер правильно", new[] { "PhoneNumber" }));
                }
            }

            return result;
        }
        private IEnumerable<ValidationResult> Email_CheckAndFix()
        {
            var result = new List<ValidationResult>();

            Email = Email.Replace(" ", "");

            var validator = new EmailAddressAttribute();
            var valid = validator.IsValid(Email);

            if (valid == false)
            {
                result.Add(new ValidationResult("Введите Email правильно", new[] { "Email" }));
            }

            return result;
        }
    }
}