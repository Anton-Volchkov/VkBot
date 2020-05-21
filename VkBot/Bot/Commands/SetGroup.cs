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
    public class SetGroup : IBotCommand
    {
        private readonly MainContext _db;
        private readonly IVkApi _vkApi;

        public string[] Aliases { get; set; } = { "группа" };
        public string Description { get; set; } = "Команда !Бот группа устанавливает группу пользователя\n" +
                                                  "Пример: !Бот группа ПЗ-50";

        public SetGroup(MainContext db, IVkApi api)
        {
            _db = db;
            _vkApi = api;
        }
        public async Task<string> Execute(Message msg)
        {
            var split = msg.Text.Split(' ', 2); // [команда, параметры]

            if (split.Length < 2)
            {
                return "Не все параметры указаны!";
            }

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Vk == msg.FromId.Value);
            var vkUser = (await _vkApi.Users.GetAsync(new[] { msg.FromId.Value })).FirstOrDefault();

            user.Group = split[1].ToLower();

            await _db.SaveChangesAsync();

            return $"{vkUser.FirstName} {vkUser.LastName}, ваша группа установлена!";

        }
    }
}
