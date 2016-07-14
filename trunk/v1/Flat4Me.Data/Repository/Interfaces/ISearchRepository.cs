using Flat4Me.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.Data.Repository.Interfaces
{
    public interface ISearchRepository
    {
        Task<List<AccommodationShortMainDTO>> SearchByCity(int cityId);
        Task<List<AccommodationShortMainDTO>> SearchByPoint(int cityId, double basePointY, double basePointX, long maxDistance);
        Task<List<AccommodationShortMainDTO>> SearchByDates(int cityId, DateTime checkInDate, DateTime checkOutDate);
    }
}
