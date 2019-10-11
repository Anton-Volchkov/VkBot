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

        public string Description { get; set; } =
            "Команда !Бот кик кикает того пользоователя чьё сообщение в чате вы переслали.\nПример: !Бот кик + пересланное сообщение\n" +
            "ВАЖНО: КОМАНДА РАБОТАЕТ ТОЛЬКО ДЛЯ АДМИНИСТРАТОРОВ!";

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
                await _db.ChatRoles.FirstOrDefaultAsync(x => x.UserVkID == forwardMessage.FromId.Value &&
                                                             x.ChatVkID == msg.PeerId.Value);

            if(kickedUser is null)
            {
                return "Данного пользователя нет или он ещё ничего не написал в этом чате!";
            }

            if ((await _db.Users.FirstOrDefaultAsync(x => x.Vk == kickedUser.UserVkID)).IsBotAdmin)
            {
                return "Вы не можете кикнуть этого человека, так как он адмминистратор бота!";
            }

            if(kickedUser.UserRole == Roles.Admin)
            {
                if(!(await _db.Users.FirstOrDefaultAsync(x => x.Vk == msg.FromId.Value)).IsBotAdmin)
                {
                    return "Вы не можете кикнуть админа!";
                }
               
            }

            try
            {
                _vkApi.Messages.RemoveChatUser((ulong)msg.PeerId.Value - 2000000000, kickedUser.UserVkID);
            }
            catch(Exception e)
            {
                return "Упс...Что-то пошло не так, возможно у меня недостаточно прав!";
            }

            _db.ChatRoles.Remove(kickedUser);
            await _db.SaveChangesAsync();

            return "Пользователь исключён!";


        }
    }
}