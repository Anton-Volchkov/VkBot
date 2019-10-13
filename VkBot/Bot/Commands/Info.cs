using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
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


        public Task<string> Execute(Message msg)
        {
            var split = msg.Text.Split(' ', 2); // [команда, параметры]

            if(Aliases.Contains(split[1].Trim().ToLower()))
            {
                return Task.FromResult(Description);
            }

            foreach(var command in Commands)
            {
                if(command.Aliases.Contains(split[1].Trim().ToLower()))
                {
                    return Task.FromResult(command.Description);
                }
            }

            return Task.FromResult($"Команда {split[1]} не найдена.");
        }
    }
}