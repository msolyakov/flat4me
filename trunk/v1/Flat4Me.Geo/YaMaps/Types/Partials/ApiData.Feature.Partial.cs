﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Flat4Me.Geo.YaMaps.Types
{
    public partial class ApiData
    {
        public partial class Feature
        {
            [JsonProperty("LocationId")]
            public long LocationId { get; set; }
        }
    }
}
