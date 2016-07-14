using Flat4me.Core.Data.Objects;
using System;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace Flat4me.Core.Data
{
    public interface ISearchRepository
    {
        Task<List<AccommodationShortMainDTO>> SearchByCity(int cityId);
        Task<List<AccommodationShortMainDTO>> SearchByDates(int cityId, DateTime checkInDate, DateTime checkOutDate);

        // Task<List<AccommodationShortMainDTO>> SearchByPoint(int cityId, double basePointY, double basePointX, long maxDistance);
    }
}
