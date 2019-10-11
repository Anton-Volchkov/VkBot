using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VkBot.Bot.Help;
using VkBot.Data.Abstractions;
using VkBot.Data.Models;
using VkNet.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands.CommandsByRoles.AdminCommands
{
    public class Kick : IBotCommand
    {
        private readonly MainContext _db;
        private readonly IVkApi _vkApi;
        private readonly RolesChecker _checker;
        public string[] Alliases { get; set; } = { "кик", "выгнать" };

        public string Description
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public Kick(MainContext db, IVkApi api, RolesChecker checker)
        {
            _db = db;
            _vkApi = api;
            _checker = checker;
        }

        public async Task<string> Execute(Message msg)
        {
            if(!await _checker.CheckAccessToRoles(msg.FromId.Value, msg.PeerId.Value, Roles.Admin))
            {
                return "Недосточно прав!";
            }

            var forwardMessage = msg.ForwardedMessages.Count == 0 ? msg.ReplyMessage : msg.ForwardedMessages[0];

            if (forwardMessage is null)
            {
                return "Нет прикреплённого сообщение пользователя которого нужно кикнуть!";
            }

            var kickedUser =
                await _db.ChatRoles.FirstOrDefaultAsync(x => x.UserID == forwardMessage.FromId.Value &&
                                                             x.ChatID == forwardMessage.PeerId.Value);

            if(kickedUser is null)
            {
                return "Такого пользователя нет в данном чате!";
            }

            if ((await _db.Users.FirstOrDefaultAsync(x => x.Vk == kickedUser.UserID)).IsBotAdmin)
            {
                return "Вы не можете кикнуть этого человека так как он адмминистратор бота!";
            }

            if(kickedUser.UserRole == Roles.Admin)
            {
                return "Вы не можете кикнуть админа!";
            }

            _vkApi.Messages.RemoveChatUser((ulong)msg.PeerId.Value, kickedUser.UserID, kickedUser.UserID);

            return "Пользователь исключён!";


        }
    }
}