using Application.Commands.Abstractions;
using Microsoft.EntityFrameworkCore;
using VkBot.Domain;
using VkBot.Domain.Models;
using VkNet.Model;

namespace Application.Commands;

public class SetCommon : IBotCommand
{
    private readonly MainContext _db;

    public SetCommon(MainContext db)
    {
        _db = db;
    }

    public string[] Aliases { get; set; } = { "запомни" };

    public string Description { get; set; } =
        "Команда !Бот запомни, запоминает пересланное вам сообщение как общее сообщение для всех кто будет его запрашивать." +
        "\nПример: !Бот запомни + пересланное сообщение ";

    public async Task<string> ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
    {
        var text = "";
        var forwardMessage = msg.ForwardedMessages.Count == 0 ? msg.ReplyMessage : msg.ForwardedMessages[0];

        if (forwardMessage is null) return "Нет сообщения!";

        text = forwardMessage.Text;

        var timeTable = await _db.Commons.FirstOrDefaultAsync();
        if (timeTable != null)
            timeTable.СommonInfo = text;
        else
            await _db.Commons.AddAsync(new Common
            {
                СommonInfo = text
            });

        await _db.SaveChangesAsync();

        return "Я запомнил сказанное!";
    }
}