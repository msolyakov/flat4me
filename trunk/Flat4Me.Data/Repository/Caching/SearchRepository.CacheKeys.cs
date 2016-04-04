using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.Data.Repository.Caching
{
    public partial class SearchRepository
    {
        /// <summary>
        /// Key for flats in specified city
        /// </summary>
        /// <remarks>
        /// {0} : city id
        /// </remarks>
        public const string SEARCH_BY_CITY_OBJECTS_KEY = "f4me.search.by-city-{0}";
        public const string SEARCH_BY_CITY_PATTERN_KEY = "f4me.search.by-city";

        /// <summary>
        /// Key for flats around specified point 
        /// </summary>
        /// <remarks>
        /// {0} : city id
        /// {1} : Y coordinate
        /// {2} : X coordinate
        /// </remarks>
        public const string SEARCH_BY_POINT_OBJECTS_KEY = "f4me.search.by-point-{0}-{1}-{2}";
        public const string SEARCH_BY_POINT_PATTERN_KEY = "f4me.search.by-point";

        /// <summary>
        /// Key for distance keys list 
        /// </summary>
        /// <remarks>
        /// No args required
        /// </remarks>
        public const string SEARCH_DISTANCE_CODES_OBJECTS_KEY = "f4me.search.distance-codes";
        public const string SEARCH_DISTANCE_CODES_PATTERN_KEY = "f4me.search.distance-codes";
    }
}
