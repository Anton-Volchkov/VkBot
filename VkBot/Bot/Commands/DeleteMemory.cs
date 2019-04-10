using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkBot.Data.Models;
using VkNet.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class DeleteMemory : IBotCommand
    {
        public string[] Alliases { get; set; } = {"забудь"};
        private readonly MainContext _db;
        private readonly IVkApi _vkApi;

        public DeleteMemory(MainContext db, IVkApi api)
        {
            _db = db;
            _vkApi = api;
        }

        public async Task<string> Execute(Message msg)
        {
            var user = (await _vkApi.Users.GetAsync(new[] { msg.FromId.Value })).FirstOrDefault();

            var UserMemory = await _db.Memories.FirstOrDefaultAsync(x => x.UserID == msg.FromId.Value);

            if (UserMemory == null)
            {
                return $"{user.FirstName} {user.LastName} вас нет в моей базе!";
            }

            UserMemory.Memory = "";
            await _db.SaveChangesAsync();
            return $"{user.FirstName} {user.LastName}, ваши данные стёрты!";
        }
    }
}
