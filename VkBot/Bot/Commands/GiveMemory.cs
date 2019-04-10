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
    public class GiveMemory : IBotCommand
    {
        public string[] Alliases { get; set; } = {"личное"};
        private readonly MainContext _db;
        private readonly IVkApi _vkApi;

        public GiveMemory(MainContext db, IVkApi api)
        {
            _db = db;
            _vkApi = api;
        }

        public async Task<string> Execute(Message msg)
        {
            string TextMemory = msg.Text.Substring(msg.Text.IndexOf("[") + 1, msg.Text.IndexOf(']') - msg.Text.IndexOf('[') - 1);
            var UserMemory = await _db.Memory.FirstOrDefaultAsync(x => x.UserID == msg.FromId.Value);
            var user = (await _vkApi.Users.GetAsync(new[] { msg.FromId.Value })).FirstOrDefault();

            if (UserMemory == null)
            {
                await _db.Memory.AddAsync(new UserMemory
                {
                   UserID = msg.FromId.Value,
                   Memory = TextMemory,
                });
            }
            else
            {
                UserMemory.Memory += $"\n {TextMemory}";
            }
            await _db.SaveChangesAsync();

            return $"{user.FirstName} {user.LastName}, я запомнил сказанное!";
        }
    }
}
