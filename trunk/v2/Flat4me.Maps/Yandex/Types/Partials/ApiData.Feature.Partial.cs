using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Flat4me.Maps.Yandex.Types
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
