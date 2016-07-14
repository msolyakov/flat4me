using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4me.Core.Data.Objects
{
    public class AccommodationShortMainDTO
    {
        public int AccommodationId { get; set; }
        public string Name { get; set; }
        public string PhotoSmallPath { get; set; }
        // Данные для фильтра
        public int MinPriceAmount { get; set; }
        // Данные для карты        
        public long LocationId { get; set; }
        public string FullAddress { get; set; }
        public double PointY { get; set; } // Широта
        public double PointX { get; set; } // Долгота
        public AccommodationDistanceDTO Distance { get; set; } 
    }
}
