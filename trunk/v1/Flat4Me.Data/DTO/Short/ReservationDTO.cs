using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.Data.DTO.Short
{
    public class ReservationDTO
    {
        public int ReservationId { get; set; }
        public int AccommodationId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public bool IsCanceled { get; set; }
        
        public DateTime Checkin { get; set; }
        public DateTime Checkout { get; set; }
        public byte Guests { get; set; }       
        public int EstimatedAmount { get; set; }
        public byte? Children { get; set; }

        public bool Compare(ReservationDTO p)
        {
            if (p == null)
                return false;

            return ReservationId == p.ReservationId
                && AccommodationId == p.AccommodationId
                && UserId == p.UserId
                && IsCanceled == p.IsCanceled
                && Checkin == p.Checkin
                && Checkout == p.Checkout
                && Guests == p.Guests
                && Children == p.Children
                && EstimatedAmount == p.EstimatedAmount;
        }
    }
}
