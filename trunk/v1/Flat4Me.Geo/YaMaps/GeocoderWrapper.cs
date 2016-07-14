using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Flat4Me.Geo.YaMaps.Types;


namespace Flat4Me.Geo.YaMaps
{
    public class GeocoderWrapper
    {
        public const string YAMAPS_GEOCODER_URI_PATTERN = "http://geocode-maps.yandex.ru/1.x/?geocode={0},+{1}&kind=house&format=json";
        public const string YAMAPS_PRECISION_EXACT = "exact";
        public const string YAMAPS_POS_SPLITTER = " ";

        private string _city;
        private string _address;

        private string _json;
        private GeocoderResponse _result;

        /// <summary>
        /// Конструктор. Принимает на вход название города и адрес.
        /// </summary>
        /// <param name="city">Например, "Самара"</param>
        /// <param name="address">Например, "Чернореченская, 16"</param>
        public GeocoderWrapper(string city, string address)
        {
            _city = sanitize(city);
            _address = sanitize(address);
        }

        #region Результаты геокодирования

        /// <summary>
        /// Результат геокодирования
        /// </summary>
        public string Json
        {
            get 
            {
                return _json;
            }
        }

        /// <summary>
        /// Объект результата геокодирования
        /// </summary>
        public GeocoderResponse Result
        {
            get 
            {
                return _result;
            }
        }
        
        #endregion

        /// <summary>
        /// Делает GET-запрос к геокодеру Яндекса и парсит результат.
        /// </summary>
        /// <param name="getExeption"></param>
        /// <returns>Флаг успешного завершения парсинга</returns>
        public bool TryGet(out Exception getExeption)
        {
            getExeption = null;
            
            try
            {
                // Запрашиваем данные с Яндекса
                _json = getJson();
                // Парсим результат в структуру объектов ответа
                _result = JsonConvert.DeserializeObject<GeocoderResponse>(_json);
            }
            catch (Exception e)
            {
                getExeption = e;
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Делает GET-запрос к геокодеру Яндекса.
        /// </summary>
        /// <returns>Данные геокодирования в формате JSON</returns>
        private string getJson()
        {
            string result;
            string uri = String.Format(YAMAPS_GEOCODER_URI_PATTERN, _city, _address);

            WebRequest req = WebRequest.Create(uri);
            using(Stream s = req.GetResponse().GetResponseStream())
            {
                using (StreamReader text = new StreamReader(s))
                {
                    result = text.ReadToEnd();
                }
            }

            return result;
        }

        private string sanitize(string s)
        {
            return s.Trim().Replace(" ", "+");
        }
    }
}
