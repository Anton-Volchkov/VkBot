﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkNet.Abstractions;
using VkNet.Model;
using WikipediaApi;


namespace VkBot.Bot.Commands
{
    public class WikiPedia : IBotCommand
    {
        public string[] Alliases { get; set; } = { "вики","википедия" };
        public string Description { get; set; } = "Команда !Бот вики вернёт вам информацию по вашему с вопросы, если она там будет, с Википедии." +
                                                  "\nПример: !Бот вики Компьютер ";

        private readonly IVkApi _vkApi;

        private readonly WikiApi _wikiApi;

        public WikiPedia(IVkApi vkApi, WikiApi wikiApi)
        {
            _vkApi = vkApi;
            _wikiApi = wikiApi;
        }
        public async Task<string> Execute(Message msg)
        {

            var user = (await _vkApi.Users.GetAsync(new[] { msg.FromId.Value })).FirstOrDefault();

            var split = msg.Text.Split(' ',2);

            string titles = split[1];

            return$"{user.FirstName} {user.LastName}, {await _wikiApi.GetWikiAnswerAsync(titles)}";
        }
    }
}