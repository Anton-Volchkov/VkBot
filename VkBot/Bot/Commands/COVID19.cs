using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoronaVirus;
using VkBot.Data.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class COVID19 : IBotCommand
    {
        private readonly CoronaInfo _coronaInfo;
        public string[] Aliases { get; set; } = { "вирус", "коронавирус" };
        public string Description { get; set; } = "Команда !Бот вирус расскажет вам общую ситуацию или ситуацию в конкретной стране связанной с COVID-19" +
        "\nПример: !Бот вирус ИЛИ Бот вирус + Страна";

        public COVID19(CoronaInfo coronaInfo)
        {
            _coronaInfo = coronaInfo;
        }
        public async Task<string> Execute(Message msg)
        {
            var split = msg.Text.Split(' ', 2); // [команда, параметры]

            return split.Length <2 ? await _coronaInfo.GetCoronaVirusInfo() : await _coronaInfo.GetCoronaVirusInfo(split[1]);

        }
    }
}
