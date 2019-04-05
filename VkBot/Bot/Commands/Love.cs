using System.Linq;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkNet.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class Love : IBotCommand
    {
        //private readonly IVkApi _vkApi;
        public string[] Alliases { get; set; } = { "любовь" };

        //public Love(IVkApi api)
        //{
        //    _vkApi = api;
        //}


        public async Task<string> Execute(Message msg)
        {
            //TODO: неиспользуемая переменная
            //var user = (await _vkApi.Users.GetAsync(new[] { msg.FromId.Value })).FirstOrDefault();

            var split = msg.Text.Split(' ', 2); // [команда, параметры]
            var param = split[1].Trim();

            return $"Процент любви {param} = {new System.Random().Next(0, 100)}%";
        }
    }
}