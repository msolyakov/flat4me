using Flat4Me.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flat4Me.Web.Portal.Models
{
    public class PhotoModel
    {
        public int? PhotoId { get; set; }
        public bool IsPrimary { get; set; }
        public string LargePath { get; set; }
        public string MediumPath { get; set; }
        public string SmallPath { get; set; }
        public string TinyPath { get; set; }

        /// <summary>
        /// Order index for view
        /// </summary>
        public int Index { get; set; }        
    }
}