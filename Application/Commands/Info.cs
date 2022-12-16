using Application.Commands.Abstractions;
using VkNet.Model;

namespace Application.Commands;

public class Info : IInfo
{
    private readonly IBotCommand[] Commands;

    public Info(IEnumerable<IBotCommand> commands)
    {
        Commands = commands.ToArray();
    }

    public string[] Aliases { get; set; } = { "инфо", "информация" };

    public string Description { get; set; } =
        "Команда !Бот инфо возвращает информацию о команде и пример ее использования." +
        "\nПример: !Бот инфо Звонок ";


    public Task<string> ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
    {
        var split = msg.Text.Split(' ', 2); // [команда, параметры]

        if (split.Length < 2) return Task.FromResult("Не все параметры указаны!");

        if (Aliases.Contains(split[1].Trim().ToLower())) return Task.FromResult(Description);

        foreach (var command in Commands)
            if (command.Aliases.Contains(split[1].Trim().ToLower()))
                return Task.FromResult(command.Description);

        return Task.FromResult($"Команда {split[1]} не найдена.");
    }
}