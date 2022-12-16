using Application.Commands.Abstractions;
using Microsoft.EntityFrameworkCore;
using VkBot.Domain;
using VkNet.Abstractions;
using VkNet.Model;

namespace Application.Commands;

public class SetGroup : IBotCommand
{
    private readonly MainContext _db;
    private readonly IVkApi _vkApi;

    public SetGroup(MainContext db, IVkApi api)
    {
        _db = db;
        _vkApi = api;
    }

    public string[] Aliases { get; set; } = { "группа" };

    public string Description { get; set; } = "Команда !Бот группа устанавливает группу пользователя\n" +
                                              "Пример: !Бот группа ПЗ-50";

    public async Task<string> ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
    {
        var split = msg.Text.Split(' ', 2); // [команда, параметры]

        if (split.Length < 2) return "Не все параметры указаны!";

        var user = await _db.Users.FirstOrDefaultAsync(x => x.Vk == msg.FromId.Value);
        var vkUser = (await _vkApi.Users.GetAsync(new[] { msg.FromId.Value })).FirstOrDefault();

        user.Group = split[1].ToLower();

        await _db.SaveChangesAsync();

        return $"{vkUser.FirstName} {vkUser.LastName}, ваша группа установлена!";
    }
}