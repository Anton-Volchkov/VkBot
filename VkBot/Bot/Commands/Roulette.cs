using System.Linq;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkNet.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class Roulette : IBotCommand
    {
        public string[] Alliases { get; set; } = { "рулетка" };
        private readonly IVkApi _vkApi;

        public Roulette(IVkApi api)
        {
            _vkApi = api;
        }

        public async Task<string> Execute(Message msg)
        {
            var user = (await _vkApi.Users.GetAsync(new[] { msg.FromId.Value })).FirstOrDefault();
            string roulette;

            //TODO: плохое решение
            if(new System.Random().Next(1, 7) == new System.Random().Next(1, 7))
            {
                roulette = $"{user.FirstName} {user.LastName} погиб(ла) в рулетке...PRESS F TO PAY RESPECT!";
            }
            else
            {
                roulette = $"{user.FirstName} {user.LastName} выжил(а) в рулетке! Поздравляем!";
            }

            return roulette;
        }
    }
}