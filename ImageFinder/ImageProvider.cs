using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using HtmlAgilityPack;
using VkBot.Extensions;


namespace ImageFinder
{
    public class ImageProvider
    {
        public async Task<List<string>> GetImagesUrl(string category)
        {

            using var fc = new FlurlClient()
                           .EnableCookies()
                           .WithHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.97 Safari/537.36");
          
            var listUrl = new List<string>();
            var EndPoint = @$"https://yandex.by/images/search?text={category.Trim().Replace(" ","+")}";

            var response = await EndPoint.WithClient(fc)
                                         .WithTimeout(TimeSpan.FromSeconds(7))
                                         .AllowAnyHttpStatus()
                                         .GetStreamAsync();

            var htmlDoc = new HtmlDocument();
            htmlDoc.Load(response);
            var node = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'serp-item__preview')]/a/img");

            if (node != null)
            {
                foreach (var link in node.Take(20).ToList().TakeRandomElements(3))
                    listUrl.Add(link.Attributes["src"].Value
                                    .Replace("//", "https://")
                                    .Replace("amp;", ""));

            }

            return listUrl;

        }
    }
}