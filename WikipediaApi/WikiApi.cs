using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json.Linq;

namespace WikipediaApi
{
    public class WikiApi
    {
        private const string EndPoint = "https://ru.wikipedia.org/w/api.php";
      
        public async Task<string> GetWikiAnswerAsync(string titles)
        {
            var response = await EndPoint
                                 .AllowAnyHttpStatus()
                                 .SetQueryParam("format", "json")
                                 .SetQueryParam("action", "query")
                                 .SetQueryParam("prop", "extracts")
                                 .SetQueryParam("exintro")
                                 .SetQueryParam("explaintext")
                                 .SetQueryParam("redirects", 1)
                                 .SetQueryParam("titles", titles)
                                 .GetAsync();
            
                //await Client.GetAsync(
                  //                    $"?format=json&action=query&prop=extracts&exintro&explaintext&redirects=1&titles={titles}");

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