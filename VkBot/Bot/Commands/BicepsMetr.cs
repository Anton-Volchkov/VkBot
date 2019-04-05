using System.Linq;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkNet.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class BicepsMetr : IBotCommand
    {
        public string[] Alliases { get; set; } = { "бицепсметр", "битка", "бицметр" };
        private readonly IVkApi _vkApi;

        public BicepsMetr(IVkApi api)
        {
            _vkApi = api;
        }

        public async Task<string> Execute(Message msg)
        {
            var user = (await _vkApi.Users.GetAsync(new[] { msg.FromId.Value })).FirstOrDefault();
            return $"{user.FirstName} {user.LastName} имеет бицепс {new System.Random().Next(10, 70)} см в обхвате! NOT BAD!";
        }
    }
}