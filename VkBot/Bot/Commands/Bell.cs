using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkBot.botlogic;
using VkBot.Data.Abstractions;
using VkNet.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class Bell : IBotCommand
    {
        private readonly IVkApi _vkApi;
        public string[] Alliases { get; set; } = { "звонок" };

        public Bell(IVkApi api)
        {
            _vkApi = api;
        }

        public async Task<string> Execute(Message msg)
        {
            var user = (await _vkApi.Users.GetAsync(new[] { msg.FromId.Value })).FirstOrDefault();
            var dt = DateTime.Now;
            return "Сейчас - " + dt.ToShortTimeString() +
                   "\n\n Расписание звонков \n 1) 8:00 - 8:45 / 8:55 - 9:40 \n 2) 9:50 - 10:35 / 10:45 - 11:30 \n 3)12:10 - 12:55 / 13:05 - 13:50 \n 4) 14:00 - 14:45 / 14:55 - 15:40 \n 5) 16:00 - 16:45 / 16:55 - 17:40 \n\n " +
                   dt.GetTime();
        }
    }
}
