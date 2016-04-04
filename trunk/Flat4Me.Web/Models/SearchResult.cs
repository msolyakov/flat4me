using Flat4Me.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flat4Me.Web.Models
{
    public class SearchResult
    {
        public List<AccommodationShortMainDTO> AccommodationList { get; set; }
        public string YaMapJson { get; set; }
    }
}