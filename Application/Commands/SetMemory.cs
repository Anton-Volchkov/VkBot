using Application.Commands.Abstractions;
using Microsoft.EntityFrameworkCore;
using VkBot.Domain;
using VkBot.Domain.Models;
using VkNet.Abstractions;
using VkNet.Model;

namespace Application.Commands;

public class SetMemory : IBotCommand
{
    private readonly MainContext _db;
    private readonly IVkApi _vkApi;

    public SetMemory(MainContext db, IVkApi api)
    {
        _db = db;
        _vkApi = api;
    }

    public string[] Aliases { get; set; } = { "личное" };

    public string Description { get; set; } =
        "Команда !Бот личное запомнит текст который вы передадите ТОЛЬКО для вас.Получить его можно через команду !бот память." +
        "\nПример: !Бот личное [Что-то личное, что запомнит бот] ";

    public async Task<string> ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
    {
        if (!(msg.Text.Contains('[') && msg.Text.Contains(']'))) return "Не все параметры указаны!";
        var textMemory = msg.Text.Substring(msg.Text.IndexOf("[") + 1,
            msg.Text.IndexOf(']') - msg.Text.IndexOf('[') - 1);
        var userMemory = await _db.Memories.FirstOrDefaultAsync(x => x.UserID == msg.FromId.Value);
        var user = (await _vkApi.Users.GetAsync(new[] { msg.FromId.Value })).FirstOrDefault();

        if (userMemory == null)
            await _db.Memories.AddAsync(new UserMemory
            {
                UserID = msg.FromId.Value,
                Memory = textMemory
            });
        else
            userMemory.Memory += $"\n {textMemory}";

        await _db.SaveChangesAsync();

        return $"{user.FirstName} {user.LastName}, я запомнил сказанное!";
    }
}