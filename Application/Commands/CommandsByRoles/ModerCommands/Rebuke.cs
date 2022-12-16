using Application.Commands.Abstractions;
using Microsoft.EntityFrameworkCore;
using Services.Helpers;
using VkBot.Domain;
using VkBot.Domain.Models;
using VkNet.Abstractions;
using VkNet.Model;
using User = VkNet.Model.User;

namespace Application.Commands.CommandsByRoles.ModerCommands;

public class Rebuke : IBotCommand
{
    private readonly IRolesHelper _checker;
    private readonly MainContext _db;

    private readonly IVkApi _vkApi;

    public Rebuke(IVkApi api, IRolesHelper checker, MainContext db)
    {
        _vkApi = api;
        _checker = checker;
        _db = db;
    }

    public string[] Aliases { get; set; } = { "варн", "выговор", "пред", "предупреждение" };

    public string Description { get; set; } =
        "Команда !Бот выговор выдаёт предупреждение тому пользоователю, чьё сообщение в чате вы переслали, или тому пользователю, к которому вы обратились по тегу.\nПример: !Бот выговор + пересланное сообщение\n" +
        "ВАЖНО: КОМАНДА РАБОТАЕТ ТОЛЬКО С ПРАВАМИ МОДЕРАТОРА И ВЫШЕ!";

    public async Task<string> ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
    {
        if (msg.PeerId.Value == msg.FromId.Value) return "Команда работает только в групповых чатах!";

        if (!await _checker.CheckAccessToCommandAsync(msg.FromId.Value, msg.PeerId.Value, Roles.Moderator, cancellationToken))
            return "Недостаточно прав!";

        var forwardMessage = msg.ForwardedMessages.Count == 0 ? msg.ReplyMessage : msg.ForwardedMessages[0];

        User rebukeUser;
        if (forwardMessage is null)
        {
            var split = msg.Text.Split(' ', 2); // [команда, параметры]

            if (split.Length < 2) return "Указаны не все параметры!";

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


        if (rebukeUser is null) return "Данного пользователя нет в этом чате!";

        if ((await _db.Users.FirstOrDefaultAsync(x => x.Vk == rebukeUser.Id, cancellationToken: cancellationToken))?.IsBotAdmin ?? false)
            return "Вы не можете дать выговор этому пользователю, так как он администратор бота!";

        if (await _checker.GetUserRoleAsync(rebukeUser.Id, msg.PeerId.Value, cancellationToken) >=
            await _checker.GetUserRoleAsync(msg.FromId.Value, msg.PeerId.Value, cancellationToken))
            if (!(await _db.Users.FirstOrDefaultAsync(x => x.Vk == msg.FromId.Value, cancellationToken: cancellationToken))?.IsBotAdmin ?? false)
                return "Вы не можете дать выговор этому пользователю т.к у него больше или такие же права!";

        var chatRebukeUser = await _db.ChatRoles.FirstOrDefaultAsync(x => x.UserVkID == rebukeUser.Id &&
                                                                          x.ChatVkID == msg.PeerId.Value, cancellationToken: cancellationToken);
        chatRebukeUser.Rebuke += 1;

        if (chatRebukeUser.Rebuke >= 3)
        {
            try
            {
                _vkApi.Messages.RemoveChatUser((ulong)msg.PeerId.Value - 2000000000, rebukeUser.Id);
            }
            catch (Exception)
            {
                return "Упс...Что-то пошло не так, возможно у меня недостаточно прав!";
            }

            var chatRole = await _db.ChatRoles.FirstOrDefaultAsync(x => x.UserVkID == rebukeUser.Id &&
                                                                        x.ChatVkID == msg.PeerId.Value, cancellationToken: cancellationToken);
            if (chatRole is not null)
            {
                _db.ChatRoles.Remove(chatRole);
            }
           
            await _db.SaveChangesAsync(cancellationToken);

            return "Пользователь набрал 3/3 предупреждений и был исключён!";
        }

        await _db.SaveChangesAsync(cancellationToken);

        return $"{rebukeUser.FirstName} {rebukeUser.LastName}, вы получили предупреждение!\n" +
               $"Всего предупреждений: {chatRebukeUser.Rebuke}/3";
    }
}