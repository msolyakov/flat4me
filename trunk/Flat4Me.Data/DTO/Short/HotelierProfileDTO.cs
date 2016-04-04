using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.Data.DTO.Short
{
    public class HotelierProfileDTO
    {
        public int UserId { get; set; }
        public int CityId { get; set; }
        public bool IsApproved { get; set; }
        
        public TimeSpan CheckinFrom { get; set; }
        public TimeSpan? CheckinTo { get; set; }
        public TimeSpan? CheckoutFrom { get; set; }
        public TimeSpan CheckoutTo { get; set; }

        public bool HasAirportTransfer { get; set; }
        public int? EstimatedAirportTransferCost { get; set; }

        public bool HasTrainTransfer { get; set; }
        public int? EstimatedTrainTransferCost { get; set; }
    }
}
