using Flat4Me.Data.DTO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Flat4Me.Web.Models
{
    public class PhotoUploadModel : IValidatableObject
    {
        public int? PhotoId { get; set; }
        public string FileName { get; set; }
        public byte[] Data { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            if (string.IsNullOrEmpty(FileName))
                result.Add(new ValidationResult("Обязательно заполните", new[] { "FileName" }));

            if (Data == null || Data.Length == 0)
                result.Add(new ValidationResult("Обязательно заполните", new[] { "Data" }));

            return result;
        }
    }
}