using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.Data.DTO
{
    public class AccommodationLocationDTO
    {
        public long LocationId { get; set; }
        public int AccommodationId { get; set; }
        public string FullAddress { get; set; }
        public double PointY { get; set; } // Широта
        public double PointX { get; set; } // Долгота
        public bool IsConfirmed { get; set; }
        public bool IsDeleted { get; set; }
        public AccommodationDistanceDTO Distance { get; set; } 
    }
}
