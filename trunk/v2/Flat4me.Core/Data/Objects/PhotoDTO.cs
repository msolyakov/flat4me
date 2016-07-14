using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4me.Core.Data.Objects
{
    public class PhotoDTO
    {
        public int? PhotoId { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsApproved { get; set; }
        public string LargePath { get; set; }
        public string MediumPath { get; set; }
        public string SmallPath { get; set; }
        public string TinyPath { get; set; }
    }
}
