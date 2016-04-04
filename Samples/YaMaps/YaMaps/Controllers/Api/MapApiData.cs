using Flat4Me.Geo.YaMaps.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace YaMaps.Controllers.Api
{
    [RoutePrefix("api/map")]
    public class MapController : ApiController
    {
        // GET api/<controller>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}
                
        //[Route("{id?}")]
        public IHttpActionResult Get()
        {
            // Point 1
            ApiData.Geometry2 g1 = new ApiData.Geometry2() 
            { 
                Type = "Point", 
                Coordinates = new double[] { 53.192959, 50.1307 } 
            };
            ApiData.Properties2 p1 = new ApiData.Properties2() { 
                BalloonContent = "Самара, ул. Чернореченская, д. 13",
                HintContent = "Чернореченская, 13" 
            };
            ApiData.Feature f1 = new ApiData.Feature() 
            { 
                Id = 1, Type = "Feature", Geometry = g1, Properties = p1 
            };

            // Point 2
            ApiData.Geometry2 g2 = new ApiData.Geometry2()
            {
                Type = "Point",
                Coordinates = new double[] { 53.201154, 50.159527 }
            };
            ApiData.Properties2 p2 = new ApiData.Properties2()
            {
                BalloonContent = "Самара, ул. Аксаковская, д. 169а",
                HintContent = "Аксаковская, 169а"
            };
            ApiData.Feature f2 = new ApiData.Feature() { Id = 2, Type = "Feature", Geometry = g2, Properties = p2 };

            // Point 3
            ApiData.Geometry2 g3 = new ApiData.Geometry2()
            {
                Type = "Point",
                Coordinates = new double[] { 53.259838, 50.20871 }
            };
            ApiData.Properties2 p3 = new ApiData.Properties2()
            {
                BalloonContent = "Самара, ул. Солнечная, д. 53",
                HintContent = "Солнечная, 53"
            };
            ApiData.Feature f3 = new ApiData.Feature() { Id = 3, Type = "Feature", Geometry = g3, Properties = p3 };
            
            ApiData result = new ApiData() {
                Type = "FeatureCollection", 
                Features = new ApiData.Feature[] { f1, f2, f3 } };

            return Ok(JsonConvert.SerializeObject(result));
        }
    }
}