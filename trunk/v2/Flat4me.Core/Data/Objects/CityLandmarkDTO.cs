using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4me.Core.Data.Objects
{
    /// <summary>
    /// Справочник мест, рядом с которыми планируется пребывание.
    /// Например - Ж/д вокзал, Клиника Бранчевского, Самарский Мед.институт.
    /// </summary>
    public class CityLandmarkDTO
    {
        public int LandmarkId { get; set; }
        public int CityId { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string FullAddress { get; set; }
        public double PointY { get; set; } // Широта
        public double PointX { get; set; } // Долгота
    }
}
