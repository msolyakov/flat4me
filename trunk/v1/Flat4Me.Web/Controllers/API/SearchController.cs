using Flat4Me.Data.DTO;
using Flat4Me.Data.Repository.Interfaces;
using Flat4Me.Data.Repository.Extensions;
using Flat4Me.Geo.YaMaps;
using Flat4Me.Web.Models;
using Newtonsoft.Json;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Flat4Me.Web.Controllers.Api
{
    [RoutePrefix("api/search")]
    public class SearchController : BaseApiController
    {
        #region Repositories

        [Inject]
        public ISearchRepository SearchRepository { get; set; }

        #endregion        
    
        [HttpPost]
        [Route("find")]
        public async Task<IHttpActionResult> Find([FromBody] SearchCriteria criteria)
        {
            List<AccommodationShortMainDTO> accList; 
            
            if (criteria == null)
                return NotFound();
          
            // 1. Получаем список элементов, в зависимости от типа поиска
            if (criteria.BasePoint != null) 
            {
                // Задана точка - ищем в БД квартиры вокруг нее
                accList = await SearchRepository.SearchByPoint(criteria.CityId,
                    criteria.BasePoint.BasePointY, criteria.BasePoint.BasePointX, criteria.BasePoint.MaxDistance);
            }
            else
            {
                // Общий поиск - возвращаем квартиры всего города
                accList = await SearchRepository.SearchByCity(criteria.CityId);
            }

            // 2. Фильтруем значения
            if (criteria.Params != null)
            { 
                List<AccommodationShortMainDTO> filteredList = await filterList(accList, criteria.Params);
                accList = filteredList;
            }

            // 3. Возвращаем результат поиска в виде модели
            return Ok(new SearchResult()
            {
                AccommodationList = accList,
                YaMapJson = JsonConvert.SerializeObject(accList.AsYaMapObject())
            });
        }

        private async Task<List<AccommodationShortMainDTO>> filterList(List<AccommodationShortMainDTO> list, ParamsCriteria parameters)
        {
            // Рассчитывам длительность
            ushort? duration = (parameters.CheckInDate != null && parameters.CheckOutDate != null)
                ? new ushort?((ushort)Math.Abs((parameters.CheckOutDate - parameters.CheckInDate).TotalDays)) : null;

            if (duration == null || (duration.HasValue && duration.Value < 1))
                duration = 1; // Подразумеваем, что идет запрос на одни неполные сутки

            return await list.FilterByParams(parameters.MinPrice, parameters.MaxPrice, parameters.GuestsCount, duration);
        }
    }
}