using Application.Commands.Abstractions;
using VkNet.Model;

namespace Application;

public interface ICommandExecutor
{
    Task<string> HandleMessageAsync(Message msg, CancellationToken cancellationToken = default);
}

public class CommandExecutor : ICommandExecutor
{
    private const string ErrorMessage = "Я не знаю такой команды =(";
    private readonly IInfo _info;
    private readonly IBotCommand[] Commands;

    public CommandExecutor(IEnumerable<IBotCommand> commands, IInfo info)
    {
        Commands = commands.ToArray();
        _info = info;
    }

    public async Task<string> HandleMessageAsync(Message msg, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(msg.Text)) return "❌Вы не указали команду!❌";
        var result = "";
        var split = msg.Text.Split(' ', 2); // [команда, параметры]
        var cmd = split[0].ToLower();

        //var cmd = msg.Text.ToLower();
        if (_info.Aliases.Contains(cmd)) return await _info.ExecuteAsync(msg, cancellationToken);

        foreach (var command in Commands)
        {
            if (!command.Aliases.Contains(cmd)) continue;

            result = await command.ExecuteAsync(msg, cancellationToken);
            break;
        }

        if (string.IsNullOrEmpty(result)) // если никакая из команд не выполнилась, посылаем сообщение об ошибке
            result = ErrorMessage;

        return result;
    }
}