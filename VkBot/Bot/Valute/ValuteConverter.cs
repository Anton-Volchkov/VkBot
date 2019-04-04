using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VkBot.Bot.Valute
{
    public class ValuteConverter
    {
        [JsonProperty("Cur_ID")]
        public long CurId { get; set; }

        [JsonProperty("Date")]
        public DateTimeOffset Date { get; set; }

        [JsonProperty("Cur_Abbreviation")]
        public string CurAbbreviation { get; set; }

        [JsonProperty("Cur_Scale")]
        public long CurScale { get; set; }

        [JsonProperty("Cur_Name")]
        public string CurName { get; set; }

        [JsonProperty("Cur_OfficialRate")]
        public double CurOfficialRate { get; set; }
    }
}
