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
    private readonly IBotCommand[] _commands;

    public CommandExecutor(IEnumerable<IBotCommand> commands, IInfo info)
    {
        _commands = commands.ToArray();
        _info = info;
    }

    public async Task<string> HandleMessageAsync(Message msg, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(msg.Text)) return "❌Вы не указали команду!❌";

        var split = msg.Text.Split(' ', 2); // [команда, параметры]
        
        var cmd = split.First().ToLower();

        if (_info.Aliases.Contains(cmd)) return await _info.ExecuteAsync(msg, cancellationToken);

        string answer = await _commands.SingleOrDefault(x => x.Aliases.Contains(cmd))?.ExecuteAsync(msg, cancellationToken);

        if (string.IsNullOrWhiteSpace(answer)) // если никакая из команд не выполнилась, посылаем сообщение об ошибке
            answer = ErrorMessage;

        return answer;
    }
}