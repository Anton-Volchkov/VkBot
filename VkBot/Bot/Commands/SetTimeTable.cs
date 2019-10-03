using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VkBot.Data.Abstractions;
using VkBot.Data.Models;
using VkNet.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class SetTimeTable : IBotCommand
    {
        private readonly MainContext _db;
        private readonly IVkApi _vkApi;

        public string[] Alliases { get; set; } = { "расписание" };
        public string Description { get; set; } = "Команда !Бот расписание устанавливает рассписание для указанной группы\n" +
                                                  "Пример: !Бот расписание ПЗ-50 1)Физ-ра 2) Математика...";
        public SetTimeTable(MainContext db, IVkApi api)
        {
            _db = db;
            _vkApi = api;
        }

        public async Task<string> Execute(Message msg)
        {
            var split = msg.Text.Split(' ', 3); // [команда, группа, параметры]

            var group = await _db.TimeTable.FirstOrDefaultAsync(x => x.Group == split[1].ToLower());

            if(group is null)
            {
                await _db.TimeTable.AddAsync(new TimeTable
                {
                    Group = split[1].ToLower(),
                    Schedule = split[2]
                });

            }
            else
            {
                group.Schedule = split[2];
            }

            await _db.SaveChangesAsync();

            return $"Расписание для группы {split[1]} установлено!";
        }
    }
}
