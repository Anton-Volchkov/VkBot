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

namespace VkBot.Bot.Commands.CommandsByRoles.EditorCommands
{
    public class SetStatus : IBotCommand
    {
        private readonly MainContext _db;
        private readonly IVkApi _vkApi;
        private readonly RolesHandler _checker;

        public SetStatus(MainContext db, IVkApi api, RolesHandler checker)
        {
            _db = db;
            _vkApi = api;
            _checker = checker;
        }
        public string[] Alliases { get; set; } = { "статус" };
        public string Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public async Task<string> Execute(Message msg)
        {
            var split = msg.Text.Split(' ', 2); // [команда, статус]

            if (!await _checker.CheckAccessToCommand(msg.FromId.Value, msg.PeerId.Value, Roles.Editor))
            {
                return "Недосточно прав!";
            }

            var forwardMessage = msg.ForwardedMessages.Count == 0 ? msg.ReplyMessage : msg.ForwardedMessages[0];

            if (forwardMessage is null)
            {
                return "Нет прикреплённого сообщение пользователя которому вы хотите установить статус!";
            }

            var statusUser =
                await _db.ChatRoles.FirstOrDefaultAsync(x => x.UserVkID == forwardMessage.FromId.Value &&
                                                             x.ChatVkID == msg.PeerId.Value);

            if (statusUser is null)
            {
                return "Данного пользователя нет или он ещё ничего не написал в этом чате!";
            }

            if ((await _db.Users.FirstOrDefaultAsync(x => x.Vk == statusUser.UserVkID)).IsBotAdmin)
            {
                if (!(await _db.Users.FirstOrDefaultAsync(x => x.Vk == msg.FromId.Value)).IsBotAdmin)
                {
                    return "Вы не можете установить статус этому пользователю, так как он адмминистратор бота!";
                }
              
            }

            if (await _checker.GetUserRole(msg.FromId.Value, msg.PeerId.Value) < statusUser.UserRole)
            {
                if (!(await _db.Users.FirstOrDefaultAsync(x => x.Vk == msg.FromId.Value)).IsBotAdmin)
                {
                    return "Вы не можете установить статус пользователю у которого больше прав доступа!";
                }
            }

            statusUser.Status = split[1];

            await _db.SaveChangesAsync();

            return "Статус установлен!";
        }
    }
}
