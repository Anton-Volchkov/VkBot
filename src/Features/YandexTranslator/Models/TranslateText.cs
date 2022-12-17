using Newtonsoft.Json;

namespace YandexTranslator.Models;

internal class TranslateText
{
    [JsonProperty("code")] public int Code;

    [JsonProperty("lang")] public string Lang;

    [JsonProperty("text")] public string[] Text;
}