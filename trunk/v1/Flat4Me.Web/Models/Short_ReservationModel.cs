using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flat4Me.Web.Models
{
    public class Short_ReservationModel : IValidatableObject
    {
        #region Db properties
        public int? ReservationId { get; set; }

        public int? AccommodationId { get; set; }

        public DateTime? Checkin { get; set; }

        public string CheckinString
        {
            get
            {
                return Checkin != null ? Checkin.Value.ToLongDateString() : string.Empty;
            }
        }

        public DateTime? Checkout { get; set; }

        public string CheckoutString
        {
            get
            {
                return Checkout != null ? Checkout.Value.ToLongDateString() : string.Empty;
            }
        }

        public byte? Guests { get; set; }

        public int? EstimatedAmount { get; set; }

        public byte? Children { get; set; }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            #region Validate simple required fields

            if (AccommodationId == null)
                result.Add(new ValidationResult("Обязательно заполните", new[] { "AccommodationId" }));

            if (Checkin == null)
                result.Add(new ValidationResult("Обязательно заполните", new[] { "Checkin" }));

            if (Checkout == null)
                result.Add(new ValidationResult("Обязательно заполните", new[] { "Checkout" }));

            if (Guests == null)
                result.Add(new ValidationResult("Обязательно заполните", new[] { "GuestsCount" }));

            if (EstimatedAmount == null)
                result.Add(new ValidationResult("Обязательно заполните", new[] { "EstimatedAmount" }));

            #endregion

            return result;
        }
        #endregion


        #region ViewFields

        public int TotalDays
        {
            get
            {
                return (int)(Checkout.Value - Checkin.Value).TotalDays;
            }
        }

        #endregion
    }
}