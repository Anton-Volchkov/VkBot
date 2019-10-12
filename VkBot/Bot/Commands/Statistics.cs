using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VkBot.Data.Abstractions;
using VkBot.Data.Models;
using VkNet.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class Statistics : IBotCommand
    {
        private readonly MainContext _db;
        private readonly IVkApi _vkApi;
        private readonly RolesHandler _checker;

        public string[] Alliases { get; set; } = { "стат", "статистика", "стата" };
        public string Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Statistics(MainContext db, IVkApi api, RolesHandler checker)
        {
            _db = db;
            _vkApi = api;
            _checker = checker;
        }

        public async Task<string> Execute(Message msg)
        {
            var forwardMessage = msg.ForwardedMessages.Count == 0 ? msg.ReplyMessage : msg.ForwardedMessages[0];

            if (forwardMessage is null)
            {
                return "Нет прикреплённого сообщение пользователя статистику которого вы хотите просмотреть!";
            }

            var user =
                await _db.ChatRoles.FirstOrDefaultAsync(x => x.UserVkID == forwardMessage.FromId.Value &&
                                                             x.ChatVkID == msg.PeerId.Value);

            if(user is null)
            {
                return "Данного пользователя нет или он ещё ничего не написал в этом чате!";
            }

            StringBuilder sb = new StringBuilder();

            var VkUser = (await _vkApi.Users.GetAsync(new[] { forwardMessage.FromId.Value })).FirstOrDefault();

            sb.AppendLine($"Статистика для пользователя - {VkUser.FirstName} {VkUser.LastName}");
            sb.AppendLine("_______________");
            sb.AppendLine($"Роль в чате: {await _checker.GetNameByRoles(user.UserRole)}");
            sb.AppendLine($"Отправлено сообщений в этом чате: {user.AmountChatMessages}");
            sb.AppendLine($"Статус: {user.Status}");

            return sb.ToString();

        }
    }
}
