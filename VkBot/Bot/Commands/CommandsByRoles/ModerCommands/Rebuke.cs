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
    public class Rebuke : IBotCommand
    {
        public string[] Aliases { get; set; } = { "варн", "выговор", "пред", "предупреждение" };
        public string Description { get; set; } =
            "Команда !Бот выговор выдаёт предупреждение тому пользоователю, чьё сообщение в чате вы переслали, или тому пользователю, к которому вы обратились по тегу.\nПример: !Бот выговор + пересланное сообщение\n" +
            "ВАЖНО: КОМАНДА РАБОТАЕТ ТОЛЬКО С ПРАВАМИ МОДЕРАТОРА И ВЫШЕ!";

        private readonly IVkApi _vkApi;
        private readonly RolesHandler _checker;
        private readonly Kick _kick;
        private readonly MainContext _db;
        
        public Rebuke(IVkApi api, RolesHandler checker, Kick kick, MainContext db)
        {
            _vkApi = api;
            _checker = checker;
            _kick = kick;
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

            User rebukeUser;
            if (forwardMessage is null)
            {
                var split = msg.Text.Split(' ', 2); // [команда, параметры]

                if (split.Length < 2)
                {
                    return "Указаны не все параметры!";
                }

                var userID = long.Parse(msg.Text.Substring(msg.Text.IndexOf("[") + 3,
                                                    msg.Text.IndexOf('|') - msg.Text.IndexOf('[') - 3));

                rebukeUser = _vkApi.Messages.GetConversationMembers(msg.PeerId.Value, new[] { "" })
                                 .Profiles.FirstOrDefault(x => x.Id == userID);

            }
            else
            {
                rebukeUser = _vkApi.Messages.GetConversationMembers(msg.PeerId.Value, new[] { "" })
                                 .Profiles.FirstOrDefault(x => x.Id == forwardMessage.FromId.Value);
            }


            if (rebukeUser is null)
            {
                return "Данного пользователя нет в этом чате!";
            }

            if ((await _db.Users.FirstOrDefaultAsync(x => x.Vk == rebukeUser.Id)).IsBotAdmin)
            {
                return "Вы не можете дать выговор этому пользователю, так как он администратор бота!";
            }

            if (await _checker.GetUserRole(rebukeUser.Id, msg.PeerId.Value) >=
               await _checker.GetUserRole(msg.FromId.Value, msg.PeerId.Value))
            {
                if (!(await _db.Users.FirstOrDefaultAsync(x => x.Vk == msg.FromId.Value)).IsBotAdmin)
                {
                    return "Вы не можете дать выговор этому пользователю т.к у него больше или такие же права!";
                }
            }

            var chatRebukeUser = await _db.ChatRoles.FirstOrDefaultAsync(x => x.UserVkID == rebukeUser.Id &&
                                                                              x.ChatVkID == msg.PeerId.Value);
            chatRebukeUser.Rebuke += 1;

            if ( chatRebukeUser.Rebuke >= 3)
            {
                try
                {
                    _vkApi.Messages.RemoveChatUser((ulong)msg.PeerId.Value - 2000000000, rebukeUser.Id);
                }
                catch (Exception)
                {
                    return "Упс...Что-то пошло не так, возможно у меня недостаточно прав!";
                }

                _db.ChatRoles.Remove(await _db.ChatRoles.FirstOrDefaultAsync(x => x.UserVkID == rebukeUser.Id &&
                                                                                  x.ChatVkID == msg.PeerId.Value));
                await _db.SaveChangesAsync();

                return "Пользователь исключён!";
            }

            await _db.SaveChangesAsync();

            return $"{rebukeUser.FirstName} {rebukeUser.LastName} вы получили выговор!\n" +
                   $"Всего выговоров: {chatRebukeUser.Rebuke}/3";
        }
    }
}
