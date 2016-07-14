using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Flat4Me.Data.DTO;
using Flat4Me.Data.Repository.Interfaces;
using Flat4Me.Geo.YaMaps;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Flat4Me.Geo;
using System.Text;
using Flat4Me.Web.Models;

namespace Flat4Me.Web.Controllers.Api
{
    [RoutePrefix("api/map")]
    public class MapController : BaseApiController
    {
        private List<CityDistanceCodeDTO> _distanceCodes = null;
        
        [Inject]
        public IMapRepository MapRepository { get; set; }

        /// <summary>
        /// Возвращает JSON, описывающий маркер для указанной квартиры
        /// </summary>
        /// <remarks>
        /// GET - api/map/5
        /// </remarks>
        /// <param name="id">Id квартиры</param>
        /// <returns>JSON формата API Я.Карт версии 2.1</returns>
        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            List<AccommodationLocationDTO> list = new List<AccommodationLocationDTO>();
            var location = await MapRepository.GetConfirmedLocation(id);
            if (location != null)
                list.Add(location);

            var yaMapObject = list.AsYaMapObject();
            string result = JsonConvert.SerializeObject(yaMapObject);

            return Ok(result);
        }

        /// <summary>
        /// Возвращает JSON, описывающий маркеры всех квартир указанного города
        /// </summary>
        /// <remarks>
        /// GET - api/map/ByCity/1
        /// </remarks>
        /// <param name="cityId">Id города</param>
        /// <returns>JSON формата API Я.Карт версии 2.1</returns>
        [HttpGet]
        [Route("byCity/{cityId:int}")]
        public async Task<IHttpActionResult> ByCity(int cityId)
        {
            List<AccommodationLocationDTO> list = await MapRepository.GetLocationListByCityId(cityId);

            var yaMapObject = list.AsYaMapObject();
            string result = JsonConvert.SerializeObject(yaMapObject);

            return Ok(result);
        }

        /// <summary>
        /// Возвращает JSON, описывающий маркеры всех квартир рядом с выбранным местом пребывания
        /// </summary>
        /// <remarks>
        /// GET - api/map/byPoint
        /// </remarks>
        /// <param name="data">Координаты точки и максимальное удаление по прямой</param>
        /// <returns>JSON формата API Я.Карт версии 2.1</returns>
        [HttpGet]
        [Route("byPoint")]
        public async Task<IHttpActionResult> ByPoint([FromBody] BasePointCriteria data)
        {
            if( data == null || data.MaxDistance == 0 )
                return NotFound();

            List<AccommodationLocationDTO> listWithDistance = new List<AccommodationLocationDTO>();
            double lowerLeftY = data.BasePointY - GeoHelper.DEGREE_Y_DELTA_2KM;
            double lowerLeftX = data.BasePointX - GeoHelper.DEGREE_X_DELTA_2KM;
            double upperRightY = data.BasePointY + GeoHelper.DEGREE_Y_DELTA_2KM;
            double upperRightX = data.BasePointX + GeoHelper.DEGREE_X_DELTA_2KM;                

            // Получаем спиcок из базы
            List<AccommodationLocationDTO> list = await MapRepository.GetLocationListByRegion(lowerLeftY, lowerLeftX, upperRightY, upperRightX);
            foreach (AccommodationLocationDTO loc in list)
            {
                // Вычисляем точное расстояние по формуле гаверсинусов
                long distance = GeoHelper.GetDistance(data.BasePointY, data.BasePointX, loc.PointY, loc.PointX);
                if(distance <= data.MaxDistance) // Найденная квартира в радиусе максимального расстояния от базовой точки
                {
                    loc.Distance = new AccommodationDistanceDTO() 
                    {
                        LocationId = loc.LocationId,
                        BasePointY = data.BasePointY,
                        BasePointX = data.BasePointX,
                        Distance = distance,
                        DistanceCode = GetCityDistanceCode(data.CityId, distance) 
                    };
                    listWithDistance.Add(loc);
                }
            }
            
            var yaMapObject = listWithDistance.AsYaMapObject();
            string result = JsonConvert.SerializeObject(yaMapObject);

            return Ok(result);
        }

        /// <summary>
        /// Возвращает JSON, описывающий все маркеры квартиры для подтверждения при заведении в личном кабинете
        /// </summary>
        /// <remarks>
        /// GET - api/map/ToConfirm/5
        /// </remarks>
        /// <param name="accomodationId">Id места пребывания</param>
        /// <returns>JSON формата API Я.Карт версии 2.1</returns>
        [HttpGet]
        [Route("toConfirm/{accomodationId:int}")]
        public async Task<IHttpActionResult> ToConfirm(int accomodationId)
        {
            List<AccommodationLocationDTO> list = await MapRepository.GetLocationList(accomodationId);

            var yaMapObject = list.AsYaMapObject();
            string result = JsonConvert.SerializeObject(yaMapObject);

            return Ok(result);
        }

        /// <summary>
        /// GET - api/map/Confirm/5
        /// </summary>
        [HttpPut]
        [Route("confirm/{locationId:int}")]
        public async Task Confirm(int locationId)
        {
            await MapRepository.ConfirmLocation(locationId);
        }

        /// <summary>
        /// GET - api/map/Geocode/5
        /// </summary>
        [HttpPut]
        [Route("geocode")]
        public async Task<IHttpActionResult> Geocode([FromBody] GeocodeModel item)
        {
            return await GeocodeParams(
                item.AccommodationId,
                item.CityName,
                item.StreetName,
                item.HouseNumber);
        }

        /// <summary>
        /// GET - api/map/GeocodeParams/5
        /// </summary>
        /// <param name="accommodationId"></param>
        [HttpPut]
        [Route("geocodeParams")]
        public async Task<IHttpActionResult> GeocodeParams(int accommodationId, string city, string street, string house)
        {
            if (string.IsNullOrEmpty(city)
                || string.IsNullOrEmpty(street)
                || string.IsNullOrEmpty(house))
                return Ok();// Empty result            

            // Формируем адресную строку
            var address = new StringBuilder();
            address.AppendFormat("{0}, {1}", street, house);

            // Получаем координаты с геокодера и преобразуем их нашу объектную модель
            var geodata = GeoHelper.Geocode(city, address.ToString());
            var locations = geodata.AsLocationList(accommodationId);

            var locationListDb = await MapRepository.GetLocationList(accommodationId);
            // Only new locations.
            var newLocationList = locations.Where(p => locationListDb.Any(pp => pp.PointX == p.PointX && pp.PointY == p.PointY) == false).ToList();
            if (newLocationList.Count > 0)
                // Сохраняем локейшены в базу
                await MapRepository.AddLocationList(newLocationList);

            return await ToConfirm(accommodationId);
        }
        
        /// <summary>
        /// Получает значение кода удаления для указанных города/расстояния
        /// </summary>
        /// <param name="cityId">ID города</param>
        /// <param name="distance">Расстояние в метрах</param>
        /// <returns></returns>
        private uint GetCityDistanceCode(int cityId, long distance)
        {
            // Загружаем коды из БД, кешируем
            if( _distanceCodes == null )
            {
                _distanceCodes = MapRepository.GetCityDistanceCodeList().Result;
            }

            // Ищем нужное значение в кеше
            CityDistanceCodeDTO obj = ( _distanceCodes != null ) ?
                (from c in _distanceCodes
                 where c.CityId == cityId && c.Distance >= distance
                 orderby c.Distance
                 select c).FirstOrDefault() : null;

            return (obj != null) ? obj.DistanceCode : 2;
        }
    }
}