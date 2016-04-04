using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.Data.DTO
{
    public class PriceDTO
    {
        public int PriceId { get; set; }        
        public byte DurationDays { get; set; }
        public int Amount { get; set; }
    }
}
