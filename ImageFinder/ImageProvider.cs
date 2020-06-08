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
    public class ImageProvider : IDisposable
    {
        public string PathToChromeDriver{ get; set; }
        public object Locker { get; set; } = new object();
        private IWebDriver Browser { get; set; }

        public ImageProvider(string pathToChromeDriver)
        {
            PathToChromeDriver = pathToChromeDriver;
            
            var options = new ChromeOptions();
          
            options.AddArguments("--no-sandbox");
            options.AddArguments("-disable-gpu");
            options.AddArguments("--headless");
            
            Browser = new ChromeDriver(PathToChromeDriver, options);
        }
        public List<string> GetImagesUrl(string category)
        {
            
            lock(Locker)
            {
                Browser.Url = $"https://yandex.by/images/search?text={category.Trim().Replace(" ","+")}";

                Browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(4);

                var elements = Browser.FindElements(By.XPath("//div[contains(@class, 'serp-item__preview')]/a/img"));

           
                var listUrl = new List<string>();

           

                foreach (var iElement in elements.ToList().TakeRandomElements(3))
                {
                    listUrl.Add(iElement.GetAttribute("src"));
                }

                Browser.Close();
                
                return listUrl;
            }

        }
        
        public void Dispose()
        {
            Browser?.Quit();
            Browser?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}