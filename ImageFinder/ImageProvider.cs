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

            using var fc = new FlurlClient().WithHeader("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X x.y; rv:42.0) Gecko/20100101 Firefox/42.0");
          
            var listUrl = new List<string>();
            var EndPoint = @$"https://yandex.by/images/search?text={category}";

            var response = await EndPoint.WithClient(fc)
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