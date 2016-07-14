using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flat4Me.Web.Models
{
    public class GetReservationListCriteriaModel: IValidatableObject
    {
        public int? AccommodationId { get; set; }
        public DateTime? DateArrivalStart { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            #region Validate simple required fields

            if (AccommodationId == null)
                result.Add(new ValidationResult("Обязательно заполните", new[] { "AccommodationId" }));       

            #endregion

            return result;        
        }
    }
}