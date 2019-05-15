using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace YandexTranslator.Models
{
    class TranslateText
    {
        [JsonProperty("code")]
        public int code;

        [JsonProperty("lang")]
        public string lang;

        [JsonProperty("text")]
        public string[] text;
    }
}
