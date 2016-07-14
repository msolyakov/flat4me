using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flat4Me.Web.Portal.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Телефон")]
        [Phone]
        public string PhoneNumber { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Телефон")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} должен быть не менее {2} символов", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароль и подтверждение пароля не совпадают")]
        public string ConfirmPassword { get; set; }
    }

    public class HotelierRegistrationViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Обязательно к заполнению")]
        [Display(Name = "Город")]
        public int? CityId { get; set; }

        public string CityName { get; set; }

        [Required]
        [Display(Name = "Заезд с")]
        public TimeSpan? CheckinFrom { get; set; }

        [Display(Name = "Заезд до (не обязательно)")]
        public TimeSpan? CheckinTo { get; set; }

        [Display(Name = "Выезд с (не обязательно)")]
        public TimeSpan? CheckoutFrom { get; set; }

        [Required]
        [Display(Name = "Выезд до")]
        public TimeSpan? CheckoutTo { get; set; }

        [Display(Name = "Трансфер из аэропорта")]
        public bool HasAirportTransfer { get; set; }
        [Display(Name = "Примерная стоимость")]
        public int? EstimatedAirportTransferCost { get; set; }

        [Display(Name = "Трансфер из ж/д вокзала")]
        public bool HasTrainTransfer { get; set; }
        [Display(Name = "Примерная стоимость")]
        public int? EstimatedTrainTransferCost { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            if (HasAirportTransfer && EstimatedAirportTransferCost == null)
            {
                result.Add(new ValidationResult("Укажите примерную стоимость трансфера", new[] { "EstimatedAirportTransferCost" }));
            }

            if (HasTrainTransfer && EstimatedTrainTransferCost == null)
            {
                result.Add(new ValidationResult("Укажите примерную стоимость трансфера", new[] { "EstimatedTrainTransferCost" }));
            }

            return result;
        }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class PhoneConfirmationViewModel
    {
        [Required]
        [Display(Name = "Введи код подтверждения")]
        public string Code { get; set; }
    }
}