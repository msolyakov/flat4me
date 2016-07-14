using Flat4Me.Core.Auth;
using Flat4Me.Data.DTO;
using Flat4Me.Data.Repository.Interfaces;
using Flat4Me.Identity;
using Ninject;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Flat4Me.Web.Controllers
{
    public class HomeController : BaseController
    {
        [Inject]
        public ICityRepository CityRepository { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> City()
        {
            // ROUTE CITY
            var cityUrl = RouteData.Values["city"];
            if (cityUrl == null || string.IsNullOrEmpty(cityUrl.ToString()) == false)
            {
                cityUrl = "samara";
                RouteData.Values["city"] = "samara";
            }

            var city = await CityRepository.GetByUrl(cityUrl.ToString());

            // CityId for JavaScript
            RouteData.Values["cityid"] = city != null ? city.CityId : 1;
            RouteData.Values["cityPointY"] = city != null ? city.PointY : 53.199933;
            RouteData.Values["cityPointX"] = city != null ? city.PointX : 50.251801;
            RouteData.Values["cityZoom"] = city != null ? city.Zoom : 12;
            
            return View();
        }

        public ActionResult Flat()
        {
            return View();
        }
    }
}