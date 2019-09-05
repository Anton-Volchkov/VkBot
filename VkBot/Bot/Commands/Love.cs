using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class Love : IBotCommand
    {
        //private readonly IVkApi _vkApi;
        public string[] Alliases { get; set; } = { "любовь" };

        public string Description { get; set; } =
            "Команда !Бот любовь является развлекательно командой, она скажет вам процент любви с чем либо по мнению бота." +
            "\nПример: !Бот любовь с Ботом ";

        //public Love(IVkApi api)
        //{
        //    _vkApi = api;
        //}


        public async Task<string> Execute(Message msg)
        {
            var split = msg.Text.Split(' ', 2); // [команда, параметры]
            var param = split[1].Trim();

            return $"Процент любви {param} = {new System.Random().Next(0, 100)}%";
        }
    }
}