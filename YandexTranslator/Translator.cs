using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace YandexTranslator
{
    public class Translator
    {
        private readonly string Token;

        private readonly HttpClient Client;

        private const string EndPoint = "https://translate.yandex.net/api/v1.5/tr.json/translate";

        public Translator(string token)
        {
            Token = token;

            Client = new HttpClient
            {
                BaseAddress = new Uri(EndPoint),
            };
        }

        public async Task<string> Translate(string text, string lang)
        {
            var response = await Client.GetAsync($"?key={Token}&text={text}&lang={lang}");

            if (!response.IsSuccessStatusCode)
            {
                return $"по вашему запросу ничего не найдено";
            }

            var translate = JsonConvert.DeserializeObject<Models.TranslateText>(await response.Content.ReadAsStringAsync());

            return string.Join("\n", translate.text);
        }
    }
}
