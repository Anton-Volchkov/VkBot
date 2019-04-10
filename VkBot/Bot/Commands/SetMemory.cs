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
    public class SetMemory : IBotCommand
    {
        public string[] Alliases { get; set; } = {"личное"};
        private readonly MainContext _db;
        private readonly IVkApi _vkApi;

        public SetMemory(MainContext db, IVkApi api)
        {
            _db = db;
            _vkApi = api;
        }

        public async Task<string> Execute(Message msg)
        {
            string textMemory = msg.Text.Substring(msg.Text.IndexOf("[") + 1, msg.Text.IndexOf(']') - msg.Text.IndexOf('[') - 1);
            var userMemory = await _db.Memories.FirstOrDefaultAsync(x => x.UserID == msg.FromId.Value);
            var user = (await _vkApi.Users.GetAsync(new[] { msg.FromId.Value })).FirstOrDefault();

            if (userMemory == null)
            {
                await _db.Memories.AddAsync(new UserMemory
                {
                   UserID = msg.FromId.Value,
                   Memory = textMemory,
                });
            }
            else
            {
                userMemory.Memory += $"\n {textMemory}";
            }
            await _db.SaveChangesAsync();

            return $"{user.FirstName} {user.LastName}, я запомнил сказанное!";
        }
    }
}
