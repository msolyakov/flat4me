using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.Data.DTO
{
    public class CityDistanceCodeDTO
    {
    	public int CityId { get; set; }
	    public uint DistanceCode { get; set; } // 0 - ~8 мин. пешком; 1 - ~15 мин. пешком; 2 - ~20 мин. на транспорте и пешком.
        public long Distance { get; set; } // Расстояние по прямой в метрах
    }
}
