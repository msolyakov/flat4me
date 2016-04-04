using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flat4Me.Web.Models
{
    /// <summary>
    /// Model for display location of object
    /// </summary>
    public class ViewLocationModel
    {
        public string FullAddress { get; set; }
        public double PointY { get; set; } // Широта
        public double PointX { get; set; } // Долгота     
    }
}