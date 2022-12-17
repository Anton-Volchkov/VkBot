using Application.Commands.Abstractions;
using VkNet.Model;

namespace Application.Commands;

public class Help : IBotCommand
{
    // public string Name { get; set; } = "Помощь";
    public string[] Aliases { get; set; } = { "команды", "помоги", "хелп", "помощь" };

    public string Description { get; set; } = "Команда !Бот команды возвращает вам список доступных команд." +
                                              "\nПример: !Бот команды ";

    public Task<string> ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
    {
        return
            Task.FromResult("Полный перечень команд вы можете посмотреть в этой статье.\nvk.com/@kerlibot-komandy");
    }
}