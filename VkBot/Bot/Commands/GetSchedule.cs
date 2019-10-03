using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VkBot.Data.Abstractions;
using VkBot.Data.Models;
using VkNet.Abstractions;
using VkNet.Model;
using User = VkBot.Data.Models.User;

namespace VkBot.Bot.Commands
{
    public class GetSchedule : IBotCommand
    {
        private readonly MainContext _db;
        private readonly IVkApi _vkApi;

        public string[] Alliases { get; set; } = { "пары", "занятия" };
        public string Description { get; set; } = "Команда !Бот расписание скажет вам расписание для установленной группы\n" +
                                                  "Пример: !Бот расписание";
        public GetSchedule(MainContext db, IVkApi api)
        {
            _db = db;
            _vkApi = api;
        }
        public async Task<string> Execute(Message msg)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Vk == msg.FromId.Value);

            if(string.IsNullOrWhiteSpace(user.Group))
            {
                return "Вы не установили группу!\n" +
                       "Установите группу командой: Бот группа + имя группы \n" +
                       "Пример: Бот группа ПЗ-50";
            }

            var schedule = (await _db.TimeTable.FirstOrDefaultAsync(x => x.Group == user.Group))?.Schedule;
            
            if (string.IsNullOrWhiteSpace(schedule))
            {
                return "Расписание нет!";
            }

            return schedule;
        }
    }
}
