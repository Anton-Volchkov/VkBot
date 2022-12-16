using System.Text;
using Application.Commands.Abstractions;
using Services.Helpers;
using VkBot.Domain;
using VkNet.Abstractions;
using VkNet.Model;

namespace Application.Commands;

public class GetTopUsersInChat : IBotCommand
{
    private readonly IRolesHelper _checker;
    private readonly MainContext _db;
    private readonly IVkApi _vkApi;

    public GetTopUsersInChat(MainContext db, IVkApi api, IRolesHelper checker)
    {
        _db = db;
        _vkApi = api;
        _checker = checker;
    }

    public string[] Aliases { get; set; } = { "топ" };

    public string Description { get; set; } =
        "Команда !Бот топ показывает топ-10 пользователей чата по кол-ву сообщений" +
        "\nПример: !Бот топ";

    public async Task<string> ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
    {
        if (msg.PeerId.Value == msg.FromId.Value) return "Команда работает только в групповых чатах!";

        var strBuilder = new StringBuilder();

        strBuilder.AppendLine("ТОП-10 участников чата по колличеству сообщений");
        strBuilder.AppendLine("_____________").AppendLine();

        var chat = _db.ChatRoles.Where(x => x.ChatVkID == msg.PeerId.Value).OrderByDescending(x => x.AmountChatMessages)
            .Take(10);

        var number = 0;
        foreach (var user in chat)
        {
            var vKUser = (await _vkApi.Users.GetAsync(new[] { (long)user.UserVkID })).FirstOrDefault();

            strBuilder.AppendLine($"{++number}. {vKUser.FirstName} {vKUser.LastName} - {user.AmountChatMessages}")
                .AppendLine();
        }

        strBuilder.AppendLine("_____________").AppendLine();

        return strBuilder.ToString();
    }
}