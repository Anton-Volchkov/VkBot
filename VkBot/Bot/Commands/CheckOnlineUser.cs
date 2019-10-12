using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkBot.Data.Models;
using VkNet.Abstractions;
using VkNet.Enums.Filters;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class CheckOnlineUser : IBotCommand
    {
        private readonly MainContext _db;
        private readonly IVkApi _vkApi;
        public string[] Alliases { get; set; } = { "онлайн" };

        public string Description { get; set; } =
            "Команда !Бот онлайн скажет вам кто онлайн в беседе.\nПример: !Бот онлайн";

        public CheckOnlineUser(MainContext db, IVkApi api)
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

            var users = _db.ChatRoles.Where(x => x.ChatVkID == msg.PeerId).ToArray();
            var userOnline = new StringBuilder();


            userOnline.AppendLine("Пользователи чата онлайн");
            userOnline.AppendLine("_____________").AppendLine();

            var chat = _vkApi.Messages.GetConversationMembers(msg.PeerId.Value, new List<string> { "online" }).Profiles
                             .Where(x => x.Online.HasValue).ToArray();

            foreach (var user in chat)
            {

                if(user.Online.Value)
                {
                    if(user.OnlineMobile.HasValue)
                    {
                        userOnline.AppendLine($"{user.FirstName} {user.LastName} (📱)");
                    }
                    else
                    {
                        userOnline.AppendLine($"{user.FirstName} {user.LastName} (💻)");
                    }
                }

            }

            userOnline.AppendLine("_____________").AppendLine();

            return userOnline.ToString();
        }
    }
}