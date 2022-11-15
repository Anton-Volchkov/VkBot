using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class Help : IBotCommand
    {
        // public string Name { get; set; } = "Помощь";
        public string[] Aliases { get; set; } = { "команды", "помоги", "хелп", "помощь" };

        public string Description { get; set; } = "Команда !Бот команды возвращает вам список доступных команд." +
                                                  "\nПример: !Бот команды ";

        public Task<string> Execute(Message msg)
        {
            return
                Task.FromResult("Полный перечень команд вы можете посмотреть в этой статье.\nvk.com/@kerlibot-komandy");
        }
    }
}