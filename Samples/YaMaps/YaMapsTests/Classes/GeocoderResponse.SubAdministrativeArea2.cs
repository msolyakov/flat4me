﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YaMaps
{

    public partial class GeocoderResponse
    {
        public class SubAdministrativeArea2
        {

            [JsonProperty("SubAdministrativeAreaName")]
            public string SubAdministrativeAreaName { get; set; }

            [JsonProperty("Locality")]
            public Locality2 Locality { get; set; }
        }
    }

}
