using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VkBot.Data.Abstractions;
using VkBot.Domain;
using VkNet.Abstractions;
using VkNet.Model;
using User = VkBot.Domain.Models.User;

namespace VkBot.Bot.Commands
{
    public class GetSchedule : IBotCommand
    {
        private readonly MainContext _db;
        private readonly IVkApi _vkApi;

        public string[] Aliases { get; set; } = { "", "" };
        public string Description { get; set; } = "Команда !Бот пары скажет вам расписание для установленной группы\n" +
                                                  "Пример: !Бот пары";
        public GetSchedule(MainContext db, IVkApi api)
        {
            _db = db;
            _vkApi = api;
        }
        public async Task<string> Execute(Message msg)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Vk == msg.FromId.Value);
            var vkUser = (await _vkApi.Users.GetAsync(new[] { msg.FromId.Value })).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(user.Group))
            {
                return $"{vkUser.FirstName} {vkUser.LastName}, Вы не установили группу!\n" +
                       "Установите группу командой: Бот группа + имя группы \n" +
                       "Пример: Бот группа ПЗ-50";
            }

            var schedule = (await _db.TimeTable.FirstOrDefaultAsync(x => x.Group == user.Group))?.Schedule;
            
            if (string.IsNullOrWhiteSpace(schedule))
            {
                return $"{vkUser.FirstName} {vkUser.LastName}, ваше расписание не заполнено!";
            }

            return schedule;
        }
    }
}
