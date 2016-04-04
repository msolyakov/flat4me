using Flat4Me.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flat4Me.Web.Models
{
    public class PriceModel
    {
        public int PriceId { get; set; }
        public int Amount { get; set; }
        public byte DurationDays { get; set; }

        public int ViewIndex { get; set; }
    }
}