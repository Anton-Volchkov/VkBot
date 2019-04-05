using System.Linq;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkNet.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class Random : IBotCommand
    {
        public string[] Alliases { get; set; } = { "рандом" };
        private readonly IVkApi _vkApi;

        public Random(IVkApi api)
        {
            _vkApi = api;
        }

        public async Task<string> Execute(Message msg)
        {
            var user = (await _vkApi.Users.GetAsync(new[] { msg.FromId.Value })).FirstOrDefault();

            return $"{user.FirstName} {user.LastName}, в промежутке от 1 до 100 выпало число - {new System.Random().Next(1, 100)}";
        }
    }
}