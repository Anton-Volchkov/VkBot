using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using VkBot.Extensions;


namespace ImageFinder
{
    public class ImageProvider
    {
        public List<string> GetImagesUrl(string category)
        {
            var listUrl = new List<string>();
            var EndPoint = @$"https://yandex.by/images/search?text={category}";

            var web = new HtmlWeb();

            var htmlDoc = web.Load(EndPoint);

            var node = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'serp-item__preview')]/a/img");

            if(node != null)
            {
                foreach(var link in node.Take(20).ToList().TakeRandomElements(3))
                    listUrl.Add(link.Attributes["src"].Value
                                    .Replace("//", "https://")
                                    .Replace("amp;", ""));

            }

            return listUrl;
        }
    }
}