using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4me.Core.Data.Objects
{
    public class CityDTO
    {
        public int CityId { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string RegionName { get; set; }
        public string CountryName { get; set; }
        public double PointY { get; set; }
        public double PointX { get; set; }
        public int Zoom { get; set; }
    }
}
