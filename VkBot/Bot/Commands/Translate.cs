using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkNet.Abstractions;
using VkNet.Model;
using YandexTranslator;

namespace VkBot.Bot.Commands
{
    public class Translate : IBotCommand
    {
        public string[] Alliases { get; set; } = { "перевод", "переводчик" };
        private readonly IVkApi _vkApi;
        private readonly Translator _translator;
        public Translate(IVkApi vkApi, Translator translator)
        {
            _vkApi = vkApi;
            _translator = translator;
        }

        public async Task<string> Execute(Message msg)
        {
            var split = msg.Text.Split(' ', 3); // [команда, параметры, текст]

            var lang = split[1].Trim().ToLower();

            var text = split[2].Trim();

            var user = (await _vkApi.Users.GetAsync(new[] { msg.FromId.Value })).FirstOrDefault();

            return $"{user.FirstName} {user.LastName}, перевод вашего текста\n\n{await _translator.Translate(text,lang)}";
        }
    }
}
