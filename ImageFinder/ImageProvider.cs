using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using VkBot.Extensions;
using VkBot.Proxy.Logic;


namespace ImageFinder
{
    public class ImageProvider 
    {
        public string PathToChromeDriver{ get; set; }
        public object Locker { get; set; } = new object();
        private readonly ChromeOptions _options = new ChromeOptions();
        private readonly ProxyProvider _proxyProvider;

        public ImageProvider(string pathToChromeDriver, ProxyProvider proxyProvider)
        {
            PathToChromeDriver = pathToChromeDriver;
            _proxyProvider = proxyProvider;
            
            _options.AddArguments("--no-sandbox");
            _options.AddArguments("-disable-gpu");
            _options.AddArguments("--headless");
       
        }
        public async Task<List<string>> GetImagesUrl(string category)
        {
            var proxyAddress = await _proxyProvider.GetRandomProxy();
          
            lock (Locker)
            {
                if(!string.IsNullOrWhiteSpace(proxyAddress))
                {
                    var proxy = new Proxy
                    {
                        HttpProxy = proxyAddress
                    };

                    _options.Proxy = proxy;
                }

                using IWebDriver browser = new ChromeDriver(PathToChromeDriver,_options);

                
                browser.Url = $"https://duckduckgo.com/?q={category.Trim().Replace(" ","+")}&t=h_&iax=images&ia=images";

                browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(4);
                
                var elements = browser.FindElements(By.XPath("//div[contains(@class, 'tile--img__media')]/span[contains(@class, 'tile--img__media__i')]/img"));
                
                var listUrl = new List<string>();

                foreach (var iElement in elements.Take(35).ToList().TakeRandomElements(3))
                {
                    listUrl.Add(iElement.GetAttribute("src"));
                }

                browser.Quit();
                
                return listUrl;
            }

        }
        
       
    }
}