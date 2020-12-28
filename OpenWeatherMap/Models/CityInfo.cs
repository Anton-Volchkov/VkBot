using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace OpenWeatherMap.Models
{
    public class CityInfo
    {
        [JsonProperty("standard")]
        public Standard Standard { get; set; }

        [JsonProperty("longt")]
        public string Longt { get; set; }

        [JsonProperty(PropertyName = "alt")]
        public Alt Alt { get; set; }

        [JsonProperty("latt")]
        public string Latt { get; set; }
    }

    public class Alt
    {
        [JsonProperty(PropertyName = "loc")]
        public List<Loc> Loc { get; set; }
    }

    public partial class Loc
    {
        [JsonProperty("longt")]
        public string Longt { get; set; }

        [JsonProperty("prov")]
        public string Prov { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("countryname")]
        public string Countryname { get; set; }

        [JsonProperty("postal")]
        public long Postal { get; set; }

        [JsonProperty("latt")]
        public string Latt { get; set; }
    }

    public class Standard
    {

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("prov")]
        public string Prov { get; set; }

        [JsonProperty("countryname")]
        public string Countryname { get; set; }

        [JsonProperty("confidence")]
        public string Confidence { get; set; }
    }

}
