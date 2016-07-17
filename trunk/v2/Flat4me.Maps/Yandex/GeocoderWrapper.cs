using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Flat4me.Maps.Yandex.Types;


namespace Flat4me.Maps.Yandex
{
    public class YandexGeocoder : BaseGeocoder, IGeocoder
    {
        public const string YAMAPS_GEOCODER_URI_PATTERN = "http://geocode-maps.yandex.ru/1.x/?geocode={0},+{1}&kind=house&format=json";
        public const string YAMAPS_PRECISION_EXACT = "exact";
        public const string YAMAPS_POS_SPLITTER = " ";

        private GeocoderResponse _result;

        public YandexGeocoder(string city, string address) : base(city, address)
        {
        }

        /// <summary>
        /// Объект результата геокодирования
        /// </summary>
        public GeocoderResponse TypedResult
        {
            get 
            {
                return Result as GeocoderResponse;
            }
        }
        
        protected override string getGeocodeUri(string city, string address)
        {
            return String.Format(YAMAPS_GEOCODER_URI_PATTERN, city, address);
        }

        protected override object deserialize(string s)
        {
            return JsonConvert.DeserializeObject<GeocoderResponse>(s);
        }
    }
}
