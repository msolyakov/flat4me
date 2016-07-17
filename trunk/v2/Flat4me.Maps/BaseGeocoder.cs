using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace Flat4me.Maps
{
    public abstract class BaseGeocoder : IGeocoder
    {
        protected string _city;
        protected string _address;

        private string _json;
        private object _result;

        /// <summary>
        /// Конструктор. Принимает на вход название города и адрес.
        /// </summary>
        /// <param name="city">Например, "Самара"</param>
        /// <param name="address">Например, "Чернореченская, 16"</param>
        public BaseGeocoder(string city, string address)
        {
            _city = sanitize(city);
            _address = sanitize(address);
        }

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
        public object Result
        {
            get
            {
                return _result;
            }
        }


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
                _result = deserialize(_json);
            }
            catch (Exception e)
            {
                getExeption = e;
                return false;
            }

            return true;
        }

        protected string getJson()
        {
            string result;
            string uri = getGeocodeUri(_city, _address);

            WebRequest req = WebRequest.Create(uri);
            using (Stream s = req.GetResponse().GetResponseStream())
            {
                using (StreamReader text = new StreamReader(s))
                {
                    result = text.ReadToEnd();
                }
            }

            return result;

        }

        protected abstract string getGeocodeUri(string city, string address);

        protected abstract object deserialize(string s);

        protected virtual string sanitize(string s)
        {
            return s.Trim().Replace(" ", "+");
        }
    }
}
