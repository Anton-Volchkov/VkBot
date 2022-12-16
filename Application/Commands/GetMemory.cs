using Application.Commands.Abstractions;
using Microsoft.EntityFrameworkCore;
using VkBot.Domain;
using VkNet.Abstractions;
using VkNet.Model;

namespace Application.Commands;

public class GetMemory : IBotCommand
{
    private readonly MainContext _db;
    private readonly IVkApi _vkApi;

    public GetMemory(MainContext db, IVkApi api)
    {
        _vkApi = api;
        _db = db;
    }

    public string[] Aliases { get; set; } = { "память" };

    public string Description { get; set; } =
        "Команда !Бот память вернёт вам ваши данные, которые вы запомнили при помощи команды !бот личное." +
        "\nПример: !Бот память ";

    public async Task<string> ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
    {
        var user = (await _vkApi.Users.GetAsync(new[] { msg.FromId.Value })).FirstOrDefault();

        var userMemory = await _db.Memories.FirstOrDefaultAsync(x => x.UserID == msg.FromId.Value);

        if (userMemory == null) return $"{user.FirstName} {user.LastName} - Я вас еще не знаю. ";

        var sendText = string.IsNullOrWhiteSpace(userMemory.Memory)
            ? "Ваших данных нет в базе!"
            : userMemory.Memory;
        return $"{user.FirstName} {user.LastName} ваши данные: \n {sendText}";
    }
}