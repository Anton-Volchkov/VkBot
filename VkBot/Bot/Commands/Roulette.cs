using System;
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

        public Task<string> Execute(Message msg)
        {
            return Task.Run(() =>
            {
                var UserName = _vkApi.Users.Get(new[] { msg.FromId.Value }).FirstOrDefault();
                var roulette = "";

                if(new Random().Next(1, 7) == new Random().Next(1, 7))
                {
                    roulette = UserName.FirstName + " " + UserName.LastName +
                               " погиб(ла) в рулетке...PRESS F TO PAY RESPECT!";
                }
                else
                {
                    roulette = UserName.FirstName + " " + UserName.LastName + " выжил(а) в рулетке! Поздравляем!";
                }

                return roulette;
            });
        }
    }
}