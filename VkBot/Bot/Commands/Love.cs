using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class Love : IBotCommand
    {
        public string[] Aliases { get; set; } = { "любовь" };

        public string Description { get; set; } =
            "Команда !Бот любовь является развлекательно командой, она скажет вам процент любви с чем либо по мнению бота." +
            "\nПример: !Бот любовь с Ботом ";

        public Task<string> Execute(Message msg)
        {
            var split = msg.Text.Split(' ', 2); // [команда, параметры]

            if (split.Length < 2)
            {
                return Task.FromResult("Не все параметры указаны!");
            }

            var param = split[1].Trim();

            return Task.FromResult($"Процент любви {param} = {new System.Random().Next(0, 100)}%");
        }
    }
}