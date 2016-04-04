using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Flat4Me.Geo;
using Flat4Me.Geo.YaMaps;
using Flat4Me.Geo.YaMaps.Types;
using Flat4Me.Data.DTO;
using System.Collections.Generic;
using System.Web.Http;
using System.Threading.Tasks;
using Flat4Me.Data.Repository.MsSql;
using System.Text;
using Newtonsoft.Json;
using Flat4Me.Data.Repository.Caching;

namespace Flat4Me.Tests
{
    [TestClass]
    public class GeoTest
    {
        static string ConnString = "Server=tcp:f4me0dev-sql.database.windows.net,1433;Database=f4me0dev;User ID=f4me0dev@f4me0dev-sql;Password=1qaz@WSX3edc;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        [TestMethod]
        public void GeoHelper_GetDistance()
        {
            /*
            Должны быть получены следующие результаты (координаты точек даны как широта/долгота, расстояние в метрах):
            # 	Точка 1             Точка 2 	        Расстояние
            1 	77.1539/-139.398 	-77.1804/-139.55 	17166029
            2 	77.1539/120.398 	77.1804/129.55 	    225883
            3 	77.1539/-120.398 	77.1804/129.55 	    2332669
            */
            long distance;

            // Вариант 1
            distance = GeoHelper.GetDistance(77.1539, -139.398, -77.1804, -139.55);
            Assert.AreEqual(17166029, distance, 1);

            // Вариант 2
            distance = GeoHelper.GetDistance(77.1539, 120.398, 77.1804, 129.55);
            Assert.AreEqual(225883, distance, 1);

            // Вариант 3
            distance = GeoHelper.GetDistance(77.1539, -120.398, 77.1804, 129.55);
            Assert.AreEqual(2332669, distance, 1);
        }

        [TestMethod]
        public void GeoHelper_Geocode()
        {
            GeocoderResponse data;

            // Варинат 1: Самара, Чернореченская, 16
            data = GeoHelper.Geocode("Самара", "Чернореченская, 16");

            Assert.IsNotNull(data.Response);
            Assert.AreEqual(data.Response.GeoObjectCollection.FeatureMember.Length, 1);

            string pos = data.Response.GeoObjectCollection.FeatureMember[0].GeoObject.Point.Pos;
            Assert.AreEqual(pos, "50.133593 53.196072");

            // Проверяем парсинг координат
            double pointY, pointX;
            if (!GeoHelper.TryParsePos(pos, out pointY, out pointX))
            {
                Assert.Fail();
            }

            Assert.AreEqual(pointY, 53.196072); // Широта
            Assert.AreEqual(pointX, 50.133593); // Долгота

            List<AccommodationLocationDTO> list = data.AsLocationList(1);
            if(list.Count != 1)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void MapController_GeocodeParams()
        {
            string result = null;
            List<AccommodationLocationDTO> list = null;
            MapRepository rep = new MapRepository();
            rep.ConnectionString = ConnString;

            int accomodationId = 1;
            string city = "Самара";
            string street = "Чернореченская";
            string house = "13";

            try
            {
                // Формируем адресную строку
                var address = new StringBuilder();
                address.AppendFormat("{0}, {1}", street, house);

                // Получаем координаты с геокодера и преобразуем их нашу объектную модель
                var geodata = GeoHelper.Geocode(city, address.ToString());
                var locations = geodata.AsLocationList(accomodationId);

                // Сохраняем локейшены в базу
                rep.AddLocationList(locations).Wait(100);

                // Получаем спсиок из базы
                list = rep.GetLocationList(accomodationId).Result;
            }
            catch (Exception e)
            {
                Assert.Fail();
            }

            if (list == null || list.Count == 0)
            {
                Assert.Fail();
            }

            var yaMapObject = list.AsYaMapObject();
            result = JsonConvert.SerializeObject(yaMapObject);

            Assert.IsTrue(result.Contains(street));
        }

        [TestMethod]
        public void SearchController_ByRegion()
        {
            SearchRepository searchRep = new SearchRepository(ConnString);

            List<AccommodationShortMainDTO> listWithDistance = new List<AccommodationShortMainDTO>();

            // Самарский Мед Институт РЕАВИЗ
            double basePointY = 53.201396;
            double basePointX = 50.106023;
            
            try
            {
                listWithDistance = searchRep.SearchByPoint(1, basePointY, basePointX, 2000).Result;
            }
            catch (Exception e)
            {
                Assert.Fail();
            }

            if (listWithDistance.Count < 1)
            {
                Assert.Fail();
            }

            Assert.AreEqual(listWithDistance[0].Distance.Distance, 1893);
        }

        [TestMethod]
        public void MapController_GetCityDistanceCode()
        {
            CityDistanceCodeDTO obj = null;
            int code = 3;

            List<CityDistanceCodeDTO> distanceCodes = null;
            MapRepository rep = new MapRepository();
            rep.ConnectionString = ConnString;

            try
            {
                distanceCodes = rep.GetCityDistanceCodeList().Result;
            }
            catch (Exception e)
            {
                Assert.Fail();
            }

            // Ищем нужное значение в кеше
            obj = (distanceCodes != null) ?
                (from c in distanceCodes
                 where c.CityId == 1 && c.Distance >= 900
                 orderby c.Distance
                 select c).FirstOrDefault() : null;

            code = (obj != null) ? (int)obj.DistanceCode : 3;
            Assert.AreEqual(code, 1);

            obj = (distanceCodes != null) ?
                (from c in distanceCodes
                 where c.CityId == 1 && c.Distance >= 400
                 orderby c.Distance
                 select c).FirstOrDefault() : null;

            code = (obj != null) ? (int)obj.DistanceCode : 3;
            Assert.AreEqual(code, 0);

            obj = (distanceCodes != null) ?
                (from c in distanceCodes
                 where c.CityId == 1 && c.Distance >= 1200
                 orderby c.Distance
                 select c).FirstOrDefault() : null;

            code = (obj != null) ? (int)obj.DistanceCode : 3;
            Assert.AreEqual(code, 2);     
        }
    }
}
