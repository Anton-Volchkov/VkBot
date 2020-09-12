using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json;
using YandexTranslator.Models;

namespace YandexTranslator
{
    public class Translator
    {
        private const string EndPoint = "https://translate.yandex.net/api/v1.5/tr.json/translate";

        private readonly string Token;

        public Translator(string token)
        {
            Token = token;
        }

        public async Task<string> Translate(string text, string lang)
        {
            var response = await EndPoint
                                 .AllowAnyHttpStatus()
                                 .SetQueryParam("key", Token)
                                 .SetQueryParam("text", text)
                                 .SetQueryParam("lang", lang)
                                 .GetAsync();

            if(!response.IsSuccessStatusCode)
            {
                return "По вашему запросу ничего не найдено";
            }

            var translate = JsonConvert.DeserializeObject<TranslateText>(await response.Content.ReadAsStringAsync());

            return string.Join("\n", translate.Text);
        }
    }
}