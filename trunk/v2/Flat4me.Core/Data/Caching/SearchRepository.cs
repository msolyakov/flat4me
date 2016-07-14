using Flat4me.Core.Caching;
using Flat4me.Core.Data.Objects;
using Flat4me.Core.Data;
using Flat4me.Core.Data.MsSql;
// using Flat4me.Geo; - TODO: serach by point should be refactored
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4me.Core.Data.Caching
{
    public partial class SearchRepository : ISearchRepository
    {
        private ICacheManager _cacheManager;
        private IAccommodationRepository _accomodationRepository;
        private IMapRepository _mapRepository;

        #region Constructor

        public SearchRepository(ICacheManager cacheManager, IAccommodationRepository accomodationRepository, IMapRepository mapRepository)
        {
            // Инициалицизуем значения через Ninject
            _cacheManager = cacheManager;
            _accomodationRepository = accomodationRepository;
            _mapRepository = mapRepository;
        }

        /// <summary>
        /// For unit tests only
        /// </summary>
        public SearchRepository(ICacheManager cacheManager, IAccommodationRepository accomodationRepository, IMapRepository mapRepository, string connectionString)
            : this(cacheManager, accomodationRepository, mapRepository)
        {
            if(_accomodationRepository is BaseRepository)
                ((BaseRepository)_accomodationRepository).ConnectionString = connectionString;

            if(_mapRepository is BaseRepository)
                ((BaseRepository)_mapRepository).ConnectionString = connectionString;
        }

        #endregion

        /// <summary>
        /// Возвращает список квартир для указанного города.
        /// Берет данные из кеша. Если таковых нет, запрашивает из БД и кладет в кеш.
        /// </summary>
        public async Task<List<AccommodationShortMainDTO>> SearchByCity(int cityId)
        {
            string cacheKey = string.Format(SearchRepository.SEARCH_BY_CITY_OBJECTS_KEY, cityId);
            return await _cacheManager.Get(cacheKey, 30, () =>
            {
                // Кешируем список квартир города на 30 минут. 
                // Т.е. новая квартира появится в поисковой выдаче максимум через 30 минут.
                return _accomodationRepository.MainList(cityId);
            });
        }

        /// <summary>
        /// Возвращает список квартир рядом с указанной точкой.
        /// Берет данные из кеша. Если таковых нет, запрашивает из БД и кладет в кеш.
        /// </summary>
        //public async Task<List<AccommodationShortMainDTO>> SearchByPoint(int cityId, double basePointY, double basePointX, long maxDistance)
        //{
        //    // Кешируем список квартир в радиусе 2 км от точки на 5 минут. 
        //    // Необходимо для уточнения параметров в следующую итерацию поиска.
        //    string cacheKey = string.Format(SearchRepository.SEARCH_BY_POINT_OBJECTS_KEY, cityId, basePointY, basePointX);
        //    List<AccommodationShortMainDTO> listWithDistance = await _cacheManager.Get(cacheKey, 5, async () =>
        //    {
        //        double lowerLeftY = basePointY - GeoHelper.DEGREE_Y_DELTA_2KM;
        //        double lowerLeftX = basePointX - GeoHelper.DEGREE_X_DELTA_2KM;
        //        double upperRightY = basePointY + GeoHelper.DEGREE_Y_DELTA_2KM;
        //        double upperRightX = basePointX + GeoHelper.DEGREE_X_DELTA_2KM;
                
        //        // Получаем из базы спиcок квартир в квадрате 4 на 4 км с центром в заданной точке
        //        List<AccommodationShortMainDTO> list = await _accomodationRepository.MainList(cityId, lowerLeftY, lowerLeftX, upperRightY, upperRightX);

        //        foreach (AccommodationShortMainDTO acc in list)
        //        {
        //            // Вычисляем точную дистанцию по формуле гаверсинусов
        //            long distance = GeoHelper.GetDistance(basePointY, basePointX, acc.PointY, acc.PointX);
        //            acc.Distance = new AccommodationDistanceDTO()
        //            {
        //                LocationId = acc.LocationId,
        //                BasePointY = basePointY,
        //                BasePointX = basePointX,
        //                Distance = distance,
        //                DistanceCode = await GetCityDistanceCode(cityId, distance)
        //            };
        //        }

        //        return list;
        //    });

        //    // Выполняем Linq-запрос для отбора квартир в пределах указанного расстояния
        //    return (from flat in listWithDistance
        //            where flat.Distance.Distance <= maxDistance
        //            select flat).ToList();
        //}

        /// <summary>
        /// Возвращает список квартир для указанного города, свободные в пределах указанных дат.
        /// Берет данные из кеша. Если таковых нет, запрашивает из БД и кладет в кеш.
        /// Не реализовано, т.к. нет календаря бронирования. Генерирует NotImplementedException.
        /// </summary>
        public async Task<List<AccommodationShortMainDTO>> SearchByDates(int cityId, DateTime checkInDate, DateTime checkOutDate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получает значение кода удаления для указанных города/расстояния
        /// </summary>
        /// <param name="cityId">ID города</param>
        /// <param name="distance">Расстояние в метрах</param>
        /// <returns></returns>
        private async Task<uint> GetCityDistanceCode(int cityId, long distance)
        {
            // Лезем за списком значений в кеш/БД
            string cacheKey = SearchRepository.SEARCH_DISTANCE_CODES_OBJECTS_KEY;
            List<CityDistanceCodeDTO> distanceCodes = await _cacheManager.Get(cacheKey, 60, () =>
            {
                return _mapRepository.GetCityDistanceCodeList();
            });

            // Ищем нужное значение в кеше
            CityDistanceCodeDTO obj = (distanceCodes != null) ?
                (from c in distanceCodes
                 where c.CityId == cityId && c.Distance >= distance
                 orderby c.Distance
                 select c).FirstOrDefault() : null;

            return (obj != null) ? obj.DistanceCode : 2;
        }
    }
}
