using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VkBot.Bot.Help;
using VkBot.Data.Abstractions;
using VkBot.Data.Models;
using VkNet.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class GetRights : IBotCommand
    {
        private readonly MainContext _db;
        private readonly IVkApi _vkApi;
        public string[] Alliases { get; set; } = { "получить" };
        public string Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public async Task<string> Execute(Message msg)
        {
            if (msg.PeerId.Value == msg.FromId.Value)
            {
                return "Команда работает только в групповых чатах!";
            }

            var split = msg.Text.Split(' ', 2); // [команда, параметры]

            if(split[1] != "права")
            {
                return "Я не знаю такой команды =(";
            }

            var chat = _vkApi.Messages.GetChat(msg.PeerId.Value - 2000000000);

            if(chat.AdminId != msg.FromId.Value)
            {
                return "Этой командой может воспользоваться только администратор сообщества!";
            }

            var admin = await _db.ChatRoles.FirstOrDefaultAsync(x => x.UserVkID == msg.FromId.Value &&
                                                                     x.ChatVkID == msg.PeerId.Value);
            admin.UserRole = Roles.GlAdmin;

            await _db.SaveChangesAsync();

            return "Права к боту выданы!";
        }
    }
}
