using Flat4me.Core.Data.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4me.Core.Data
{
    public interface IMapRepository
    {
        Task<AccommodationLocationDTO> GetConfirmedLocation(int accommodationId);
        Task<List<AccommodationLocationDTO>> GetLocationList(int accommodationId);
        Task<List<AccommodationLocationDTO>> GetLocationListByLandmarkId(int landmarkId);
        Task<List<AccommodationLocationDTO>> GetLocationListByCityId(int cityId);
        Task<List<AccommodationLocationDTO>> GetLocationListByRegion(double lowerLeftY, double lowerLeftX, double upperRightY, double upperRightX);
        Task<List<CityDistanceCodeDTO>> GetCityDistanceCodeList();

        Task AddLocation(AccommodationLocationDTO location);
        Task AddLocationList(List<AccommodationLocationDTO> location);
        Task ConfirmLocation(long locationId);

        Task<List<CityLandmarkDTO>> GetLandmarkList(int cityId);
    }
}
