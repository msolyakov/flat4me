using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4me.Core.Data.Objects
{
    /// <summary>
    /// Краткий профайл хотельера, когда админ получает список хотельеров на подтверждение
    /// </summary>
    public class HotelierProfileUnapprovedDTO
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PhotoTinyPath { get; set; }        

        public int CityId { get; set; }
        public string CityName { get; set; }
    }
}
