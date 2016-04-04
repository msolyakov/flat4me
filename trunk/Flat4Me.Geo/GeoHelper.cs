using Flat4Me.Geo.YaMaps;
using Flat4Me.Geo.YaMaps.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Flat4Me.Geo
{
    public class GeoHelper
    {
        /// <summary>
        /// Усредненный радиус Земли (метры)
        /// </summary>
        public const long EARTH_RADIUS_METERS = 6372795;

        /// <summary>
        /// Дельта координат для широты, примерно соответвующая 2 км.
        /// </summary>
        public const double DEGREE_Y_DELTA_2KM = 0.018;

        /// <summary>
        /// Дельта координат для долготы, примерно соответвующая 2 км.
        /// </summary>
        public const double DEGREE_X_DELTA_2KM = 0.0301;

        /// <summary>
        /// Вычисляет расстояние между двумя точками с координатами, переданными в качестве аргументов
        /// </summary>
        /// <param name="lat1">Широта 1-й точки</param>
        /// <param name="long1">Долгота 1-й точки</param>
        /// <param name="lat2">Широта 2-й точки</param>
        /// <param name="long2">Долгота 2-й точки</param>
        /// <returns>
        /// Расстояние между точками в метрах в метрах
        /// </returns>
        /// <remarks>
        /// Вычисление производится по модифицированной формуле гаверсинусов:
        /// http://gis-lab.info/qa/great-circles.html
        /// http://wiki.gis-lab.info/w/Вычисление_расстояния_и_начального_азимута_между_двумя_точками_на_сфере
        /// </remarks>
        public static long GetDistance(double lat1, double long1, double lat2, double long2)
        {
            // Переводим координаты в радианы
            double lat1R = ToRadians(lat1); 
            double long1R = ToRadians(long1);
            double lat2R = ToRadians(lat2);
            double long2R = ToRadians(long2);
            
            // Вычисляем косинусы и синусы широт и разницы долгот
            double cl1 = Math.Cos(lat1R);
            double cl2 = Math.Cos(lat2R);
            double sl1 = Math.Sin(lat1R);
            double sl2 = Math.Sin(lat2R);
            double deltaR = long2R - long1R;
            double cdelta = Math.Cos(deltaR);
            double sdelta = Math.Sin(deltaR);

            // Вычисление расстояния по длине большого круга
            double y = Math.Sqrt(Math.Pow(cl2 * sdelta, 2) + Math.Pow(cl1 * sl2 - sl1 * cl2 * cdelta, 2));
            double x = sl1 * sl2 + cl1 * cl2 * cdelta;
            double dist = Math.Atan2(y,x) * EARTH_RADIUS_METERS;

            return Convert.ToInt64(dist);
        }

        /// <summary>
        /// Переводит градусы в радианы
        /// </summary>
        public static double ToRadians(double angle)
        {
            return angle * (Math.PI/180.0);
        }

        /// <summary>
        /// Делает запрос к геокодеру Яндекса и возвращает GeocoderResponse.
        /// </summary>
        /// <param name="city">Например, "Самара"</param>
        /// <param name="address">Например, "Чернореченская, 16"</param>
        /// <returns>Объект GeocoderResponse</returns>
        public static GeocoderResponse Geocode(string city, string address)
        {
            GeocoderWrapper wrp = new GeocoderWrapper(city, address);
            
            Exception e;
            if (!wrp.TryGet(out e))
            {
                throw e;
            }

            return wrp.Result;
        }

        /// <summary>
        /// Парсит строку с координатами, полученную от геокодера
        /// </summary>
        /// <param name="pos">Строка вида "50.133593 53.196072"</param>
        /// <param name="pointY">Широта</param>
        /// <param name="pointX">Долгота</param>
        /// <returns>Признак успешного парсинга</returns>
        public static bool TryParsePos( string pos, out double pointY, out double pointX )
        {
            pointX = 0.0;
            pointY = 0.0;
            
            if(String.IsNullOrEmpty(pos))
                return false;
            
            try
            {
                // 1. Сплитим строку
                string[] coords = pos.Split(
                    new string[] { GeocoderWrapper.YAMAPS_POS_SPLITTER },
                    StringSplitOptions.RemoveEmptyEntries);
                // 2. Проверяем полученный массив
                if (coords.Length < 2)
                    return false; 
                
                // 3. Парсим значения
                pointX = XmlConvert.ToDouble(coords[0]);
                pointY = XmlConvert.ToDouble(coords[1]);
            }
            catch
            {
                pointX = 0.0;
                pointY = 0.0;
                return false;
            }

            return true;
        }
    }
}
