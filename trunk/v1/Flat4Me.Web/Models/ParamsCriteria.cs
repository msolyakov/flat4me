using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flat4Me.Web.Models
{
    public class ParamsCriteria
    {
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public ushort? GuestsCount { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}