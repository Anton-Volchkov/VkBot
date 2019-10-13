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
        public string Description { get; set; } =
            "Команда !Бот получить права выдает в боте права главного администратора создателю беседы" +
            "\nПример: !Бот получить права ";

        public GetRights(MainContext db, IVkApi api)
        {
            _db = db;
            _vkApi = api;
        }
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

            var chat = _vkApi.Messages.GetConversationMembers(msg.PeerId.Value, new List<string> { "is_admin" }).Items.Where(x => x.IsAdmin).Select(x => x.MemberId).ToArray();

            if(!chat.Contains(msg.FromId.Value))
            {
                return "Этой командой может воспользоваться только администратор!";
            }

            foreach(var item in chat)
            {
                var admin = await _db.ChatRoles.FirstOrDefaultAsync(x => x.UserVkID == msg.FromId.Value &&
                                                                         x.ChatVkID == msg.PeerId.Value);
                admin.UserRole = Roles.GlAdmin;
            }

            await _db.SaveChangesAsync();

            return "Права к боту выданы!";
        }
    }
}
