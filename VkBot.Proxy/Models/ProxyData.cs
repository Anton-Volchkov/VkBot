using System;
using Newtonsoft.Json;

namespace VkBot.Proxy.Models
{
    public class ProxyData
    {
        [JsonProperty("ipPort")]
        public string IpPort { get; set; }

        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("port")]
      
        public long Port { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("last_checked")]
        public DateTimeOffset LastChecked { get; set; }

        [JsonProperty("proxy_level")]
        public string ProxyLevel { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("speed")]
       
        public long Speed { get; set; }

        [JsonProperty("support")]
        public ProxyInfo Support { get; set; }
    }
}
