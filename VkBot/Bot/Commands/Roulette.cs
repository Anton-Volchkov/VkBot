using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkNet.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class Roulette: IBotCommand
    {
        public Roulette(IVkApi api)
        {
            _vkApi = api;
        }
        private readonly IVkApi _vkApi;
        Random rnd = new Random();
        public string[] Alliases { get; set; } = { "рулетка" };

        public Task<string> Execute(Message msg)
        {
            return Task.Run(() =>
            {

                var UserName = _vkApi.Users.Get(new[] {msg.FromId.Value }).FirstOrDefault();
                string roulette;
                if (rnd.Next(1,7)== rnd.Next(1,7))
                {
                    roulette = UserName.FirstName + " " + UserName.LastName + " выжил(а) в рулетке! Поздравляем!";
                }
                else
                {
                    roulette = UserName.FirstName + " " + UserName.LastName + " погиб в рулетке...PRESS F TO PAY RESPECT!";
                }
                return roulette;
            });
        }
    }
}
