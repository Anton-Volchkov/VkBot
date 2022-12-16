using Application.Commands.Abstractions;
using Microsoft.EntityFrameworkCore;
using Services.Helpers;
using VkBot.Domain;
using VkBot.Domain.Models;
using VkNet.Model;

namespace Application.Commands.CommandsByRoles.EditorCommands;

public class SetStatus : IBotCommand
{
    private readonly IRolesHelper _checker;
    private readonly MainContext _db;

    public SetStatus(MainContext db, IRolesHelper checker)
    {
        _db = db;
        _checker = checker;
    }

    public string[] Aliases { get; set; } = { "статус" };

    public string Description { get; set; } =
        "Команда !Бот статус устанавливает указанный статус пользователю, чьё сообщение в чате вы переслали.\nПример: !Бот статус ТЕСТОВЫЙ СТАТУС + пересланное сообщение\n" +
        "ВАЖНО: КОМАНДА РАБОТАЕТ ТОЛЬКО С ПРАВАМИ РЕДАКТОРА И ВЫШЕ!";

    public async Task<string> ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
    {
        if (msg.PeerId.Value == msg.FromId.Value) return "Команда работает только в групповых чатах!";

        var split = msg.Text.Split(' ', 2); // [команда, статус]

        if (split.Length < 2) return "Не все параметры указаны!";

        if (!await _checker.CheckAccessToCommandAsync(msg.FromId.Value, msg.PeerId.Value, Roles.Editor, cancellationToken))
            return "Недостаточно прав!";

        var forwardMessage = msg.ForwardedMessages.Count == 0 ? msg.ReplyMessage : msg.ForwardedMessages[0];

        if (forwardMessage is null)
            forwardMessage = msg; //если пересланного сообщения нет, то статус ставится тому кто это писал.

        var statusUser =
            await _db.ChatRoles.FirstOrDefaultAsync(x => x.UserVkID == forwardMessage.FromId.Value &&
                                                         x.ChatVkID == msg.PeerId.Value, cancellationToken: cancellationToken);

        if (statusUser is null) return "Данного пользователя нет или он ещё ничего не написал в этом чате!";

        if ((await _db.Users.FirstOrDefaultAsync(x => x.Vk == statusUser.UserVkID, cancellationToken: cancellationToken))?.IsBotAdmin ?? false)
            if (!(await _db.Users.FirstOrDefaultAsync(x => x.Vk == msg.FromId.Value, cancellationToken: cancellationToken))?.IsBotAdmin ?? false)
                return "Вы не можете установить статус этому пользователю, так как он администратор бота!";

        if (await _checker.GetUserRoleAsync(msg.FromId.Value, msg.PeerId.Value, cancellationToken) < statusUser.UserRole)
            if (!(await _db.Users.FirstOrDefaultAsync(x => x.Vk == msg.FromId.Value, cancellationToken: cancellationToken))?.IsBotAdmin ?? false)
                return "Вы не можете установить статус пользователю у которого больше прав доступа!";

        statusUser.Status = split[1];

        await _db.SaveChangesAsync(cancellationToken);

        return "Статус установлен!";
    }
}