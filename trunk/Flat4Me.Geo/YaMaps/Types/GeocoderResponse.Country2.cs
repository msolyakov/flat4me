﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Flat4Me.Geo.YaMaps.Types
{

    public partial class GeocoderResponse
    {
        public class Country2
        {

            [JsonProperty("AddressLine")]
            public string AddressLine { get; set; }

            [JsonProperty("CountryNameCode")]
            public string CountryNameCode { get; set; }

            [JsonProperty("CountryName")]
            public string CountryName { get; set; }

            [JsonProperty("AdministrativeArea")]
            public AdministrativeArea2 AdministrativeArea { get; set; }
        }
    }

}