using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkBot.Data.Models;
using VkNet;
using VkNet.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class MailingWeather : IBotCommand
    {
        public string[] Alliases { get; set; } =  {"подписка","отписка"};
        private readonly MainContext _db;
        private readonly IVkApi _vkApi;
        public MailingWeather(MainContext db, IVkApi vkApi)
        {
            _db = db;
            _vkApi = vkApi;
        }
        public async Task<string> Execute(Message msg)
        {
            var split = msg.Text.Split(' ', 2); // [команда, параметры]
            var command = split[0].Trim();
            var user = await _db.Users.FirstOrDefaultAsync((x) => x.Vk == msg.FromId.Value);
            var vkUser = (await _vkApi.Users.GetAsync(new[] { msg.FromId.Value })).FirstOrDefault();

            if (command == "отписка")
            {
                user.Weather = false;
                await _db.SaveChangesAsync();

                return  $"{vkUser.FirstName} {vkUser.LastName}, вы отписались от рассылки погоды!";
            }
            else
            {
                var city = split[1].Trim().ToLower();
                city = char.ToUpper(city[0]) + city.Substring(1);
                user.City = city;
                user.Weather = true;
                await _db.SaveChangesAsync();
                return $"{vkUser.FirstName} {vkUser.LastName}, подписка на рассылку погоды в городе {city} успешно оформлена!\n" +
                       $"ВАЖНО: Для корректной рассылки погоды у вас должен быть диалог с ботом. Если его нет, пожалуйста напишите ему любоое сообщение https://vk.com/kerlibot.";
            }


        }
    }
}
