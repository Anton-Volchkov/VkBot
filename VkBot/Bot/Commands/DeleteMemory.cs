using Microsoft.EntityFrameworkCore;
using VkBot.Data.Abstractions;
using VkBot.Domain;
using VkNet.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands;

public class DeleteMemory : IBotCommand
{
    private readonly MainContext _db;
    private readonly IVkApi _vkApi;

    public DeleteMemory(MainContext db, IVkApi api)
    {
        _db = db;
        _vkApi = api;
    }

    public string[] Aliases { get; set; } = { "забудь" };

    public string Description { get; set; } =
        "Команда !Бот забудь забывает все введённые вами данный(при помощи команды !Бот личное)." +
        "\nПример: !Бот забудь";

    public async Task<string> Execute(Message msg)
    {
        var user = (await _vkApi.Users.GetAsync(new[] { msg.FromId.Value })).FirstOrDefault();

        var userMemory = await _db.Memories.FirstOrDefaultAsync(x => x.UserID == msg.FromId.Value);

        if (userMemory == null) return $"{user.FirstName} {user.LastName} вас нет в моей базе!";

        userMemory.Memory = "";
        await _db.SaveChangesAsync();
        return $"{user.FirstName} {user.LastName}, ваши данные стёрты!";
    }
}