using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flat4Me.Web.Portal.Models
{
    public class SearchCriteria
    {
        public int CitytId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public ushort? GuestsCount { get; set; }
        public ushort? ChildrenCount { get; set; }
        public uint? MinPrice { get; set; }
        public uint? MaxPrice { get; set; }
        public BasePointCriteria BasePoint { get; set; }
    }
}