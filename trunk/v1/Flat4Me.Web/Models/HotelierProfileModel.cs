using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flat4Me.Web.Models
{
    public class HotelierProfileModel
    {
        #region Db properties

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

        #endregion

        #region View properties

        public bool HasCheckinTo
        {
            get
            {
                return CheckinTo.HasValue;
            }
        }
        public bool HasCheckoutFrom
        {
            get
            {
                return CheckoutFrom.HasValue;
            }
        }

        public string CheckinFromString { get { return CheckinFrom.ToString(@"hh\:mm"); } }
        public string CheckinToString { get { return CheckinTo.HasValue ? CheckinTo.Value.ToString(@"hh\:mm") : string.Empty; } }
        public string CheckoutFromString { get { return CheckoutFrom.HasValue ? CheckoutFrom.Value.ToString(@"hh\:mm") : string.Empty; } }
        public string CheckoutToString { get { return CheckoutTo.ToString(@"hh\:mm"); } }

        #endregion
    }
}