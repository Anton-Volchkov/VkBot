using Newtonsoft.Json;

namespace VkBot.Proxy.Models
{
    public class ProxyInfo
    {
        [JsonProperty("https")]
        public bool? Https { get; set; }

        [JsonProperty("get")]
        public bool? Get { get; set; }

        [JsonProperty("post")]
        public bool? Post { get; set; }

        [JsonProperty("cookies")]
        public bool? Cookies { get; set; }

        [JsonProperty("referer")]
        public bool? Referer { get; set; }

        [JsonProperty("user_agent")]
        public bool? UserAgent { get; set; }

        [JsonProperty("google")]
        public bool? Google { get; set; }
    }
}
