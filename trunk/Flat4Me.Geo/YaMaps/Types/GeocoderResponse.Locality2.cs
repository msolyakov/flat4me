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
        public class Locality2
        {

            [JsonProperty("LocalityName")]
            public string LocalityName { get; set; }

            [JsonProperty("Thoroughfare")]
            public Thoroughfare2 Thoroughfare { get; set; }
        }
    }

}
