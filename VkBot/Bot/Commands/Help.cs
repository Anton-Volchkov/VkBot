using System.Text;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class Help : IBotCommand
    {
        // public string Name { get; set; } = "Помощь";
        public string[] Alliases { get; set; } = { "команды", "помоги", "хелп" };

        public Task<string> Execute(Message msg)
        {
            return Task.Run(() =>
            {
                var strBuilder = new StringBuilder();
                strBuilder.AppendLine("***КОМАНДЫ БОТА****");
                strBuilder.AppendLine("Перед каждой командой нужно ставить восклицательный знак.");
                strBuilder.AppendLine("Пример: !Команда");
                strBuilder.AppendLine("_____________").AppendLine();
                strBuilder.AppendLine("Бот звонок");
                strBuilder.AppendLine("Бот бибаметр");
                strBuilder.AppendLine("Бот бицепсметр");
                strBuilder.AppendLine("Бот рандом");
                strBuilder.AppendLine("Бот русская рулетка");
                strBuilder.AppendLine("Бот запомни расписание [В скобках указать расписание]");
                strBuilder.AppendLine("Бот расписание").AppendLine();
                strBuilder.AppendLine("_____________");

                return strBuilder.ToString();
            });
        }
    }
}