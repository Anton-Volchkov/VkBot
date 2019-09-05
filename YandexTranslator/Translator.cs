using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YandexTranslator.Models;

namespace YandexTranslator
{
    public class Translator
    {
        private const string EndPoint = "https://translate.yandex.net/api/v1.5/tr.json/translate";

        private readonly HttpClient Client;
        private readonly string Token;

        public Translator(string token)
        {
            Token = token;

            Client = new HttpClient
            {
                BaseAddress = new Uri(EndPoint)
            };
        }

        public async Task<string> Translate(string text, string lang)
        {
            var response = await Client.GetAsync($"?key={Token}&text={text}&lang={lang}");

            if(!response.IsSuccessStatusCode)
            {
                return "по вашему запросу ничего не найдено";
            }

            var translate = JsonConvert.DeserializeObject<TranslateText>(await response.Content.ReadAsStringAsync());

            return string.Join("\n", translate.text);
        }
    }
}