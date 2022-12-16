using Newtonsoft.Json;

namespace VkBot.Proxy.Models;

public class ProxyModel
{
    [JsonProperty("data")] public ProxyData[] Data { get; set; }

    [JsonProperty("count")] public long Count { get; set; }
}