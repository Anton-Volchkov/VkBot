using System.Text;
using Application.Commands.Abstractions;
using VkBot.Domain;
using VkNet.Abstractions;
using VkNet.Model;

namespace Application.Commands;

public class CheckOnlineUser : IBotCommand
{
    private readonly MainContext _db;
    private readonly IVkApi _vkApi;

    public CheckOnlineUser(MainContext db, IVkApi api)
    {
        _db = db;
        _vkApi = api;
    }

    public string[] Aliases { get; set; } = { "онлайн" };

    public string Description { get; set; } =
        "Команда !Бот онлайн скажет вам кто онлайн в беседе.\nПример: !Бот онлайн";

    public Task<string> ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
    {
        if (msg.PeerId.Value == msg.FromId.Value) return Task.FromResult("Команда работает только в групповых чатах!");

        User[] chat;
        try
        {
            chat = _vkApi.Messages.GetConversationMembers(msg.PeerId.Value, new[] { "online" })
                .Profiles
                .Where(x => x.Online.HasValue).ToArray();
        }
        catch (Exception)
        {
            return Task.FromResult(
                "Что-то пошло не так, возможно у меня не хвататет прав. Установите мне права администратора и попробуйте снова.");
        }

        var strBuilder = new StringBuilder();

        strBuilder.AppendLine("Пользователи чата онлайн");
        strBuilder.AppendLine("_____________").AppendLine();


        foreach (var user in chat)
            if (user.Online.HasValue && user.Online.Value)
            {
                if (user.OnlineMobile.HasValue)
                    strBuilder.AppendLine($"{user.FirstName} {user.LastName} (📱)");
                else
                    strBuilder.AppendLine($"{user.FirstName} {user.LastName} (💻)");
            }

        strBuilder.AppendLine("_____________").AppendLine();

        return Task.FromResult(strBuilder.ToString());
    }
}