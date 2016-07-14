using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4me.Core.Data.Objects
{
    /// <summary>
    /// Расстояние от квартиры (посуточная аренда) до базовой точки, указанной в свойствах.
    /// </summary>
    /// <remarks>
    /// Хранится код удаленности, примерно соответствующий следующим значениям:
    /// 0 - 8 мин. пешком; 
    /// 1 - 15 мин. пешком; 
    /// 2 - 20 мин. на транспорте и пешком.
    /// </remarks>
    public class AccommodationDistanceDTO
    {
        public long LocationId { get; set; }
        public double BasePointY { get; set; } // Широта
        public double BasePointX { get; set; } // Долгота
        public long Distance { get; set; } // Расстояние по прямой в метрах        
        public uint DistanceCode { get; set; } // 0 - ~8 мин. пешком; 1 - ~15 мин. пешком; 2 - ~20 мин. на транспорте и пешком.
    }
}
