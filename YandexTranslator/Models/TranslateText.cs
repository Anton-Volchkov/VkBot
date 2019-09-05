using Newtonsoft.Json;

namespace YandexTranslator.Models
{
    internal class TranslateText
    {
        [JsonProperty("code")] public int code;

        [JsonProperty("lang")] public string lang;

        [JsonProperty("text")] public string[] text;
    }
}