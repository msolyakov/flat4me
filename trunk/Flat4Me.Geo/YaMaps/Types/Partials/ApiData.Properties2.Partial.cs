using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Flat4Me.Geo.YaMaps.Types
{
    public partial class ApiData
    {
        public partial class Properties2
        {
            [JsonProperty("locationId")]
            public long LocationId { get; set; }

            [JsonProperty("distance")]
            public long Distance { get; set; }
        }
    }
}
