﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VkBot.Bot.Help;
using VkBot.Data.Abstractions;
using VkBot.Data.Models;
using VkNet.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands.CommandsByRoles.AdminCommands
{
    public class SetRole : IBotCommand
    {
        private readonly MainContext _db;
        private readonly IVkApi _vkApi;
        private readonly RolesHandler _checker;
        public SetRole(MainContext db, IVkApi api, RolesHandler checker)
        {
            _db = db;
            _vkApi = api;
            _checker = checker;
        }

        public string[] Alliases { get; set; } = { "роль" };
        public string Description { get; set; } =
            "Команда !Бот роль устанавливает роль пользователю, чьё сообщение в чате вы переслали.\nПример: !Бот кик + пересланное сообщение\n" +
            "ВАЖНО: КОМАНДА РАБОТАЕТ ТОЛЬКО ДЛЯ АДМИНИСТРАТОРОВ!";

        public async Task<string> Execute(Message msg)
        {
            var split = msg.Text.Split(' ', 2); // [команда, роль]

            if (!await _checker.CheckAccessToCommand(msg.FromId.Value, msg.PeerId.Value, Roles.Admin))
            {
                return "Недосточно прав!";
            }

            var forwardMessage = msg.ForwardedMessages.Count == 0 ? msg.ReplyMessage : msg.ForwardedMessages[0];

            if (forwardMessage is null)
            {
                return "Нет прикреплённого сообщение пользователя которому вы хотите выдать роль!";
            }

            var roleUser =
                await _db.ChatRoles.FirstOrDefaultAsync(x => x.UserVkID == forwardMessage.FromId.Value &&
                                                             x.ChatVkID == msg.PeerId.Value);

            if (roleUser is null)
            {
                return "Данного пользователя нет или он ещё ничего не написал в этом чате!";
            }

            var role = await _checker.GetRolesByName(split[1].ToLower());

            if(role == Roles.RoleNotFound)
            {
                return $"Роли {split[1]} не существует!";
            }

            if(role == Roles.Admin)
            {
                if (!await _checker.CheckAccessToCommand(msg.FromId.Value, msg.PeerId.Value, Roles.GlAdmin))
                {
                    return "Для установки данной роли у вас недостаточно прав!";
                }
            }
            else if(role == Roles.GlAdmin)
            {
                if(!(await _db.Users.FirstOrDefaultAsync(x => x.Vk == msg.FromId.Value)).IsBotAdmin)
                {
                    return "Для установки данной роли у вас недостаточно прав!";
                }
            }

            if(await _checker.GetUserRole(msg.FromId.Value, msg.PeerId.Value) <= roleUser.UserRole)
            {
                if (!(await _db.Users.FirstOrDefaultAsync(x => x.Vk == msg.FromId.Value)).IsBotAdmin)
                {
                    return "Вы не можете изменить роль пользователю у которого точно такие права доступа или выше!";
                }
            }

            roleUser.UserRole = role;

            await _db.SaveChangesAsync();

            return "Роль установлена!";
        }
    }
}
