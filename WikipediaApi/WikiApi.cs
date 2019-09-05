using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WikipediaApi
{
    public class WikiApi
    {
        private const string EndPoint = "https://ru.wikipedia.org/w/api.php";
        private readonly HttpClient Client;

        public WikiApi()
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri(EndPoint)
            };
        }

        public async Task<string> GetWikiAnswerAsync(string titles)
        {
            var response =
                await Client.GetAsync(
                                      $"?format=json&action=query&prop=extracts&exintro&explaintext&redirects=1&titles={titles}");

            if(!response.IsSuccessStatusCode)
            {
                return "по вашему запросу ничего не найдено";
            }

            var jo = JObject.Parse(await response.Content.ReadAsStringAsync());
            JToken token = jo["query"] as JObject;
            token = token["pages"] as JObject;
            token = token.FirstOrDefault();
            var answer = token.FirstOrDefault(x => x.Value<string>("extract") != "");
            return $"ответ по запросу {titles}\n\n{answer.Value<string>("extract")}";
        }
    }
}