using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flat4Me.Web.Models
{
    public class SearchCriteria
    {
        public int CityId { get; set; }
        public BasePointCriteria BasePoint { get; set; }
        public ParamsCriteria Params { get; set; }
    }
}