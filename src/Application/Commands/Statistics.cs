using System.Text;
using Application.Commands.Abstractions;
using Microsoft.EntityFrameworkCore;
using Services.Helpers;
using VkBot.Domain;
using VkNet.Abstractions;
using VkNet.Model;

namespace Application.Commands;

public class Statistics : IBotCommand
{
    private readonly IRolesHelper _checker;
    private readonly MainContext _db;
    private readonly IVkApi _vkApi;

    public Statistics(MainContext db, IVkApi api, IRolesHelper checker)
    {
        _db = db;
        _vkApi = api;
        _checker = checker;
    }

    public string[] Aliases { get; set; } = { "стат", "статистика", "стата" };

    public string Description { get; set; } =
        "Команда !Бот стат + пересланное сообщение скажет вам о статистике пользователя в этом чате, чьё сообщение вы переслали" +
        "\nПример: !Бот стат + пересланное сообщение";

    public async Task<string> ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
    {
        if (msg.PeerId.Value == msg.FromId.Value) return "Команда работает только в групповых чатах!";


        var forwardMessage = (msg.ForwardedMessages.Any() ? msg.ForwardedMessages[0] : msg.ReplyMessage) ?? msg;

        var userInChat =
            await _db.ChatRoles.FirstOrDefaultAsync(x => x.UserVkID == forwardMessage.FromId.Value &&
                                                         x.ChatVkID == msg.PeerId.Value, cancellationToken: cancellationToken);

        if (userInChat is null) return "Данный пользователь ещё ничего не написал в этом чате!";

        var VkUser = (await _vkApi.Users.GetAsync(new[] { forwardMessage.FromId.Value })).FirstOrDefault();

        var botUser = await _db.Users.FirstOrDefaultAsync(x => x.Vk == forwardMessage.FromId.Value, cancellationToken: cancellationToken);

        var nameUserGroup = string.IsNullOrWhiteSpace(botUser.Group)
            ? "Группа не установлена."
            : botUser.Group.ToUpper();

        var sb = new StringBuilder();

        var status = string.IsNullOrWhiteSpace(userInChat.Status) ? "Не установлен" : userInChat.Status;

        sb.AppendLine($"Статистика для пользователя - {VkUser.FirstName} {VkUser.LastName}");
        sb.AppendLine("_______________").AppendLine();
        sb.AppendLine($"Роль в чате: {_checker.GetNameByRole(userInChat.UserRole)}").AppendLine();
        sb.AppendLine($"Отправлено сообщений в этом чате: {userInChat.AmountChatMessages}").AppendLine();
        sb.AppendLine($"Статус: {status}").AppendLine();
        sb.AppendLine($"Группа: {nameUserGroup}").AppendLine();
        sb.AppendLine($"Предупреждений: {userInChat.Rebuke}/3").AppendLine();

        sb.AppendLine("_______________");

        return sb.ToString();
    }
}