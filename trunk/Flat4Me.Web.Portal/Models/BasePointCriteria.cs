using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flat4Me.Web.Portal.Models
{
    public class BasePointCriteria
    {
        public int CityId { get; set; }
        public double BasePointY { get; set; }
        public double BasePointX { get; set; }
        public long MaxDistance { get; set; }
    }
}