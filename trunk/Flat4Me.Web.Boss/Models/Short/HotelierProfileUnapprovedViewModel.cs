using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flat4Me.Web.Boss.Models.Short
{
    public class HotelierProfileUnapprovedViewModel
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