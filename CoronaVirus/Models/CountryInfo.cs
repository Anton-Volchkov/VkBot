using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CoronaVirus.Models
{
    public class CountryInfo
    {
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("cases")]
        public long? Cases { get; set; }

        [JsonProperty("todayCases")]
        public long? TodayCases { get; set; }

        [JsonProperty("deaths")]
        public long? Deaths { get; set; }

        [JsonProperty("todayDeaths")]
        public long? TodayDeaths { get; set; }

        [JsonProperty("recovered")]
        public long? Recovered { get; set; }

        [JsonProperty("active")]
        public long? Active { get; set; }

        [JsonProperty("critical")]
        public long? Critical { get; set; }

        [JsonProperty("totalTests")]
        public long? TotalTests { get; set; }
    }
}
