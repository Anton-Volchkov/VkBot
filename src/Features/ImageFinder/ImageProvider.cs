using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageFinder.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using VkBot.Extensions;
using VkBot.Proxy.Logic;

namespace ImageFinder;

public class ImageProvider
{
    private readonly ChromeOptions _options = new();
    private readonly ProxyProvider _proxyProvider;
    public object Locker { get; set; } = new();

    public ImageProvider(ProxyProvider proxyProvider)
    {
        _proxyProvider = proxyProvider;

        _options.AddArguments("--no-sandbox");
        _options.AddArguments("-disable-gpu");
        _options.AddArguments("--headless");
    }

    public async Task<List<string>> GetImagesUrl(string category, Browser nameBrowser)
    {
        var proxyAddress = await _proxyProvider.GetRandomProxy();
        var currentQueryUrl = "";
        var currentXPath = "";

        lock (Locker)
        {
            if (!string.IsNullOrWhiteSpace(proxyAddress))
            {
                var proxy = new Proxy
                {
                    HttpProxy = proxyAddress
                };

                _options.Proxy = proxy;
            }

            if (nameBrowser == Browser.Yandex)
            {
                currentQueryUrl = $"https://yandex.by/images/search?text={category.Trim().Replace(" ", "+")}";
                currentXPath = "//div[contains(@class, 'serp-item__preview')]/a/img";
            }
            else if (nameBrowser == Browser.DuckDuckGo)
            {
                currentQueryUrl =
                    $"https://duckduckgo.com/?q={category.Trim().Replace(" ", "+")}&t=h_&iax=images&ia=images";
                currentXPath =
                    "//div[contains(@class, 'tile--img__media')]/span[contains(@class, 'tile--img__media__i')]/img";
            }

            //Docker container required if run without docker compose(docker run --name selenium -d -p 4444:4444 selenium/standalone-chrome)
            using IWebDriver browser = new RemoteWebDriver(new Uri("http://selenium:4444"), _options);

            browser.Url = currentQueryUrl;

            browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(4);

            var elements = browser.FindElements(By.XPath(currentXPath));

            var listUrl = new List<string>();

            foreach (var iElement in elements.Take(nameBrowser == Browser.Yandex ? elements.Count : 10)
                         .TakeRandomElements(3))
                listUrl.Add(iElement.GetAttribute("src"));

            browser.Quit();

            return listUrl;
        }
    }
}