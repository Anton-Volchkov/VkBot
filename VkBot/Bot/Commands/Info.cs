using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class Info : IInfo
    {
        public string[] Alliases { get; set; } = { "инфо", "информация" };
        public string Description { get; set; } = "Команда !Бот инфо возвращает информацию о команде и пример ее использования." +
                                                  "\nПример: !Бот инфо Звонок ";

        private readonly IBotCommand[] Commands;

        public Info(IEnumerable<IBotCommand> commands)
        {  
                Commands = commands.ToArray(); 
        }
    

        public async Task<string> Execute(Message msg)
        {
            var split = msg.Text.Split(' ', 2); // [команда, параметры]

            if (split[1].Trim().ToLower() == "инфо" || split[1].Trim().ToLower() == "информация")
            {
                return this.Description;
            }

            foreach (var command in Commands)
            {
                if (command.Alliases.Contains(split[1].Trim().ToLower()))
                {
                    return command.Description;
                }
            }
            return $"Комманды {split[1]} не найдено.";
        }
    }
}
