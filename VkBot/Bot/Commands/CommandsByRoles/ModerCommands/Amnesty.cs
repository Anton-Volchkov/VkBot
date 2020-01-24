using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VkBot.Data.Abstractions;
using VkBot.Data.Models;
using VkNet.Abstractions;
using VkNet.Model;
using User = VkNet.Model.User;

namespace VkBot.Bot.Commands.CommandsByRoles.ModerCommands
{
    public class Amnesty : IBotCommand
    {
        public string[] Aliases { get; set; } = { "простить", "амнистия" };
        public string Description { get; set; } =
            "Команда !Бот амнистия снимает предупреждение с того пользоователя, чьё сообщение в чате вы переслали, или тому пользователю, к которому вы обратились по тегу.\nПример: !Бот амнистия + пересланное сообщение\n" +
            "ВАЖНО: КОМАНДА РАБОТАЕТ ТОЛЬКО С ПРАВАМИ МОДЕРАТОРА И ВЫШЕ!";

        private readonly RolesHandler _checker;
        private readonly IVkApi _vkApi;
        private readonly MainContext _db;
        public Amnesty(RolesHandler checker, IVkApi api, MainContext db)
        {
            _checker = checker;
            _vkApi = api;
            _db = db;
        }
        public async Task<string> Execute(Message msg)
        {
            if (msg.PeerId.Value == msg.FromId.Value)
            {
                return "Команда работает только в групповых чатах!";
            }

            if (!await _checker.CheckAccessToCommand(msg.FromId.Value, msg.PeerId.Value, Roles.Moderator))
            {
                return "Недосточно прав!";
            }

            var forwardMessage = msg.ForwardedMessages.Count == 0 ? msg.ReplyMessage : msg.ForwardedMessages[0];

            User amnestyUser;
            if (forwardMessage is null)
            {
                var split = msg.Text.Split(' ', 2); // [команда, параметры]

                if (split.Length < 2)
                {
                    return "Указаны не все параметры!";
                }

                var userID = long.Parse(msg.Text.Substring(msg.Text.IndexOf("[") + 3,
                                                    msg.Text.IndexOf('|') - msg.Text.IndexOf('[') - 3));

                amnestyUser = _vkApi.Messages.GetConversationMembers(msg.PeerId.Value, new[] { "" })
                                 .Profiles.FirstOrDefault(x => x.Id == userID);

            }
            else
            {
                amnestyUser = _vkApi.Messages.GetConversationMembers(msg.PeerId.Value, new[] { "" })
                                 .Profiles.FirstOrDefault(x => x.Id == forwardMessage.FromId.Value);
            }


            if (amnestyUser is null)
            {
                return "Данного пользователя нет в этом чате!";
            }

            if (await _checker.GetUserRole(amnestyUser.Id, msg.PeerId.Value) >=
               await _checker.GetUserRole(msg.FromId.Value, msg.PeerId.Value))
            {
                if (!(await _db.Users.FirstOrDefaultAsync(x => x.Vk == msg.FromId.Value)).IsBotAdmin)
                {
                    return "Вы не можете простить этого пользователя т.к у него больше или такие же права!";
                }
            }

            var chatAmnestyUser = await _db.ChatRoles.FirstOrDefaultAsync(x => x.UserVkID == amnestyUser.Id &&
                                                                               x.ChatVkID == msg.PeerId.Value);
            chatAmnestyUser.Rebuke = 0;
            
            await _db.SaveChangesAsync();

            return $"{amnestyUser.FirstName} {amnestyUser.LastName} с вас сняли все выговоры!\n" +
                   $"Всего выговоров 0/3";
        }
    }
}
