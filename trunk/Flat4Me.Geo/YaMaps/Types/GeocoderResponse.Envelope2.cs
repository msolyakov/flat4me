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
        public class Envelope2
        {

            [JsonProperty("lowerCorner")]
            public string LowerCorner { get; set; }

            [JsonProperty("upperCorner")]
            public string UpperCorner { get; set; }
        }
    }

}
