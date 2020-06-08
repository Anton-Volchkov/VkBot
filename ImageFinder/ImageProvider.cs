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
        public string PathToChromeDriver{ get; set; }

        public ImageProvider(string pathToChromeDriver)
        {
            PathToChromeDriver = pathToChromeDriver;
        }
        public List<string> GetImagesUrl(string category)
        {
            var options = new ChromeOptions();
          
            options.AddArguments("--no-sandbox");
            options.AddArguments("-disable-gpu");
            options.AddArguments("--headless");

            
            IWebDriver driver = new ChromeDriver(PathToChromeDriver, options);

            driver.Url = $"https://yandex.by/images/search?text={category.Trim().Replace(" ","+")}";

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(4);

            var elements = driver.FindElements(By.XPath("//div[contains(@class, 'serp-item__preview')]/a/img"));

           
            var listUrl = new List<string>();

           

            foreach (var iElement in elements.ToList().TakeRandomElements(3))
            {
                listUrl.Add(iElement.GetAttribute("src"));
            }

            driver.Quit();


            return listUrl;
           
        }
    }
}