﻿using Application.Commands.Abstractions;
using Microsoft.EntityFrameworkCore;
using Services.Helpers;
using VkBot.Domain;
using VkBot.Domain.Models;
using VkNet.Abstractions;
using VkNet.Model;

namespace Application.Commands.CommandsByRoles.AdminCommands;

public class BlackList : IBotCommand
{
    private readonly IRolesHelper _checker;
    private readonly MainContext _db;
    private readonly IVkApi _vkApi;

    public BlackList(MainContext db, IVkApi vkApi, IRolesHelper checker)
    {
        _db = db;
        _vkApi = vkApi;
        _checker = checker;
    }

    public string[] Aliases { get; set; } = { "чс" };

    public string Description { get; set; } =
        "Команда !Бот чс добавляет пользователя, чьё сообщение в чате вы переслали, или тому пользователю, к которому вы обратились по тегу, в черный список.\nПример: !Бот чс + пересланное сообщение\n" +
        "ВАЖНО: КОМАНДА РАБОТАЕТ ТОЛЬКО С ПРАВАМИ АДМИНИСТРАТОРА ИЛИ ВЫШЕ!";


    public async Task<string> ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
    {
        if (msg.PeerId.Value == msg.FromId.Value) return "Команда работает только в групповых чатах!";

        if (!await _checker.CheckAccessToCommandAsync(msg.FromId.Value, msg.PeerId.Value, Roles.Admin, cancellationToken))
            return "Недостаточно прав!";

        var forwardMessage = msg.ForwardedMessages.Count == 0 ? msg.ReplyMessage : msg.ForwardedMessages[0];

        long? kickedUserId;
        if (forwardMessage is null)
        {
            var split = msg.Text.Split(' ', 2); // [команда, параметры]

            if (split.Length < 2) return "Указаны не все параметры!";

            kickedUserId = long.Parse(msg.Text.Substring(msg.Text.IndexOf("[") + 3,
                msg.Text.IndexOf('|') - msg.Text.IndexOf('[') - 3));
        }
        else
        {
            kickedUserId = forwardMessage.FromId.Value;
        }


        if ((await _db.Users.FirstOrDefaultAsync(x => x.Vk == kickedUserId, cancellationToken: cancellationToken))?.IsBotAdmin ?? false)
            return "Вы не можете добавить пользователя в черный список, так как он администратор бота!";

        if (await _checker.GetUserRoleAsync(kickedUserId, msg.PeerId.Value, cancellationToken) >=
            await _checker.GetUserRoleAsync(msg.FromId.Value, msg.PeerId.Value, cancellationToken))
            if (!(await _db.Users.FirstOrDefaultAsync(x => x.Vk == msg.FromId.Value, cancellationToken: cancellationToken))?.IsBotAdmin ?? false)
                return "Вы не можете добавить пользователя в черный спиисок т.к у него больше или такие же права!";

        var userAlreadyInBlackList = await
            _db.BlackList.FirstOrDefaultAsync(x => x.ChatVkId == msg.PeerId && x.UserVkId == kickedUserId, cancellationToken: cancellationToken);


        if (userAlreadyInBlackList is null)
        {
            await _db.BlackList.AddAsync(new VkBot.Domain.Models.BlackList
            {
                ChatVkId = msg.PeerId.Value,
                UserVkId = kickedUserId.Value
            }, cancellationToken);

            await _db.SaveChangesAsync(cancellationToken);
        }

        try
        {
            _vkApi.Messages.RemoveChatUser((ulong)msg.PeerId.Value - 2000000000, kickedUserId);
            var chatRole = await _db.ChatRoles.FirstOrDefaultAsync(x => x.UserVkID == kickedUserId &&
                                                                        x.ChatVkID == msg.PeerId.Value,
                cancellationToken: cancellationToken);

            if (chatRole is not null)
            {
                _db.ChatRoles.Remove(chatRole);
                await _db.SaveChangesAsync(cancellationToken);
            }
    
        }
        catch (Exception)
        {
            return "Пользователь добавлен в черный список!";
        }


        return "Пользователь добавлен в черный список!";
    }
}