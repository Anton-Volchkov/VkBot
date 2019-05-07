using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkBot.Data.Models;
using VkNet;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class MailingWeather : IBotCommand
    {
        public string[] Alliases { get; set; } =  {"подписка","отписка"};
        private readonly MainContext _db;
        private readonly VkApi _vkApi;
        public MailingWeather(MainContext db, VkApi vkApi)
        {
            _db = db;
            _vkApi = vkApi;
        }
        public async Task<string> Execute(Message msg)
        {
            var split = msg.Text.Split(' ', 2); // [команда, параметры]
            var city = split[1].Trim();
            var command = split[0].Trim();

            var user = await _db.Users.FirstOrDefaultAsync((x) => x.Vk == msg.FromId.Value);
            var vkUser = (await _vkApi.Users.GetAsync(new[] { msg.FromId.Value })).FirstOrDefault();
            user.Weather = command == "подписка" ? true : false;

            return command == "подписка" ? $"{vkUser.FirstName} {vkUser.LastName}, подписка на рассылку по городу {city} успешно оформлена!"
                                        : $"{vkUser.FirstName} {vkUser.LastName}, вы отписались от рассылки погоды!";
                                                                                                                                              
        }
    }
}
