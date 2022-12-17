using Newtonsoft.Json;

namespace CoronaVirus.Models;

public class FullInfo
{
    [JsonProperty("cases")] public int Cases { get; set; }

    [JsonProperty("deaths")] public int Deaths { get; set; }

    [JsonProperty("recovered")] public int Recovered { get; set; }
}