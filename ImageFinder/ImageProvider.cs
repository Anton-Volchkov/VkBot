using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using VkBot.Extensions;


namespace ImageFinder
{
    public class ImageProvider
    {
      
        public async Task<List<string>> GetImagesUrl(string category)
        {
            Console.WriteLine("Тест селениума на хероку");
            using (IWebDriver driver = new ChromeDriver(@"/app/.chromedriver/bin/chromedriver"))
            {
                Console.WriteLine("Браузер запущен");

                driver.Url = "https://yandex.by/images/search?text=котики}";
                
                driver.Manage().Window.Maximize();

                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                var s =  driver.FindElements(By.XPath("//div[contains(@class, 'serp-item__preview')]/a/img"));
            
                var listUrl = new List<string>();

                foreach (var iElement in s.ToList().TakeRandomElements(3))
                {
                    listUrl.Add(iElement.GetAttribute("src"));
                }

                return listUrl;
            }
            //using var fc = new FlurlClient()
            //               .EnableCookies()
            //               .WithHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.97 Safari/537.36");
          
            
            //var EndPoint = @$"https://yandex.by/images/search?text={category.Trim().Replace(" ","+")}";

            //var response = await EndPoint.WithClient(fc)
            //                             .WithTimeout(TimeSpan.FromSeconds(7))
            //                             .AllowAnyHttpStatus()
            //                             .GetStreamAsync();

            //var htmlDoc = new HtmlDocument();
            //htmlDoc.Load(response);
            //var node = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'serp-item__preview')]/a/img");

            //if (node != null)
            //{
            //    foreach (var link in node.Take(20).ToList().TakeRandomElements(3))
            //        listUrl.Add(link.Attributes["src"].Value
            //                        .Replace("//", "https://")
            //                        .Replace("amp;", ""));

            //}

            //return listUrl;

        }
    }
}