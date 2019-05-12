using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WikipediaApi
{
   public class WikiApi
   {
        private readonly HttpClient Client;

        private const string EndPoint = "https://ru.wikipedia.org/w/api.php";

        public WikiApi()
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri(EndPoint),
            };
        }

        public async Task<string> GetWikiAnswerAsync(string titles)
        {
            var response = await Client.GetAsync($"?format=json&action=query&prop=extracts&exintro&explaintext&redirects=1&titles={titles}");

            if (!response.IsSuccessStatusCode)
            {
                return $"по вашему запросу ничего не найдено";
            }

            JObject jo = JObject.Parse(await response.Content.ReadAsStringAsync());
            JToken token = (jo["query"] as JObject);
            token = (token["pages"] as JObject);
            token = token.FirstOrDefault();
            JToken answer = token.FirstOrDefault(x => x.Value<string>("extract") != "");
            return $"ответ по запросу {titles}\n\n{answer.Value<string>("extract")}";
        }
   }
}
