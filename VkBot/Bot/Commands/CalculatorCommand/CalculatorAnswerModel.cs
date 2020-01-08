using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VkBot.Bot.Commands.CalculatorCommand
{
    public class CalculatorAnswerModel
    {
        [JsonProperty("operation")]
        public string Operation { get; set; }

        [JsonProperty("expression")]
        public string Expression { get; set; }

        [JsonProperty("result")]
        public string Result { get; set; }
    }
}
