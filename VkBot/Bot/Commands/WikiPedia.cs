using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkNet.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class WikiPedia : IBotCommand
    {
        public string[] Alliases { get; set; } = { "вики","википедия" };

        private readonly IVkApi _vkApi;

        private readonly HttpClient Client;

        private const string EndPoint = "https://ru.wikipedia.org/w/api.php";

        public WikiPedia(IVkApi vkApi)
        {
            _vkApi = vkApi;

            Client = new HttpClient
            {
                BaseAddress = new Uri(EndPoint),
            };
        }
        public async Task<string> Execute(Message msg)
        {
            var user = (await _vkApi.Users.GetAsync(new[] { msg.FromId.Value })).FirstOrDefault();

            var split = msg.Text.Split(' ',2);

            string titles = split[1];

            var response = await Client.GetAsync($"?format=json&action=query&prop=extracts&exintro&explaintext&redirects=1&titles={titles}");

            if (!response.IsSuccessStatusCode)
            {
                return $"{user.FirstName} {user.LastName}, по вашему запросу ничего не найдено";
            }

            JObject jo = JObject.Parse(await response.Content.ReadAsStringAsync());
            JToken token = (jo["query"] as JObject);
            token = (token["pages"] as JObject);
            token = token.FirstOrDefault();
            JToken answer = token.FirstOrDefault(x => x.Value<string>("extract") != "");
            return $"{user.FirstName} {user.LastName}, ответ по запросу {titles}\n\n" + answer.Value<string>("extract");
        }
    }
}
