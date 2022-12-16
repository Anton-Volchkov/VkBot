using Application.Commands.Abstractions;
using Microsoft.EntityFrameworkCore;
using VkBot.Domain;
using VkNet.Model;

namespace Application.Commands;

public class GetCommon : IBotCommand
{
    private readonly MainContext _db;

    public GetCommon(MainContext db)
    {
        _db = db;
    }

    public string[] Aliases { get; set; } = { "общее" };

    public string Description { get; set; } =
        "Команда !Бот общее вернёт вам общее для все сообщение, которое было установлено при помощи команды(!Бот общее)." +
        "\nПример: !Бот общее";

    public async Task<string> ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
    {
        const string scheduleEmpty = "Общего сообщения нет!";
        var sendText = await _db.Commons.FirstOrDefaultAsync();

        return sendText?.СommonInfo ?? scheduleEmpty;
    }
}