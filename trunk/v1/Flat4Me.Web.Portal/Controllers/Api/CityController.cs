using Flat4Me.Data.Repository.Interfaces;
using Flat4Me.Web.Portal.Models;
using Ninject;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Flat4Me.Web.Portal.Controllers.Api
{
    [RoutePrefix("api/city")]
    public class CityController : BaseApiController
    {
        [Inject]
        public ICityRepository CityRepository { get; set; }

        // GET: api/city/cityName or countryName or regionName
        [Route("{criteria?}")]
        public async Task<IHttpActionResult> Get(string criteria)
        {
            if (string.IsNullOrEmpty(criteria))
            {
                return Ok();
            }

            var findResult = await CityRepository.Find(criteria);
            var result = findResult.Select(
                p => new TypeaheadModel 
                { 
                    Id = p.CityId,
                    Name = string.Format("{0}, {1}", p.RegionName, p.Name)
                });

            return Ok(result);
        }
    }
}