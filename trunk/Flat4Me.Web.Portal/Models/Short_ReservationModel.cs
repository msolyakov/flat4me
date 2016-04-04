using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flat4Me.Web.Portal.Models
{
    public class Short_ReservationModel
    {
        public int ReservationId { get; set; }
        public int AccommodationId { get; set; }
        public int UserId { get; set; }
        public DateTime Checkin { get; set; }
        public string CheckinString
        {
            get
            {
                return Checkin.ToLongDateString();
            }
        }
        public DateTime Checkout { get; set; }
        public string CheckoutString
        {
            get
            {
                return Checkout.ToLongDateString();
            }
        }
        public byte Guests { get; set; }        
        public int EstimatedAmount { get; set; }
        public byte? Children { get; set; }
    }
}