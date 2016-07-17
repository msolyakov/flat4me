using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4me.Maps
{
    interface IGeocoder
    {
        /// <summary>
        /// Json geocoder's rezult
        /// </summary>
        string Json { get; }

        /// <summary>
        /// Object with geocoder's rezult
        /// </summary>
        object Result { get; }

        /// <summary>
        /// Performs GET-request to Geocoder and и parse result.
        /// </summary>
        /// <param name="getExeption"></param>
        /// <returns>Success parse flag</returns>
        bool TryGet(out Exception getExeption);
    }
}
