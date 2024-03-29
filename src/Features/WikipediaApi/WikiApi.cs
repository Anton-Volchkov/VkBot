﻿using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json.Linq;

namespace WikipediaApi;

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

        if (!response.ResponseMessage.IsSuccessStatusCode) return "по вашему запросу ничего не найдено";

        var jo = JObject.Parse(await response.GetStringAsync());
        JToken token = jo["query"] as JObject;
        token = token["pages"] as JObject;
        token = token.FirstOrDefault();
        var answer = token.FirstOrDefault(x => x.Value<string>("extract") != "");
        return $"ответ по запросу {titles}\n\n{answer.Value<string>("extract")}";
    }
}