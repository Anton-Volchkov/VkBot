using System;
using System.Net.Http;
using System.Threading.Tasks;
using CurrencyConverter.Models;
using Newtonsoft.Json;

namespace CurrencyConverter
{
    public class CurrencyInfo
    {
        private const string EndPoint = "https://www.nbrb.by/API/ExRates/Rates/";
        private readonly HttpClient Client;

        public CurrencyInfo()
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri(EndPoint)
            };
        }

        public async Task<Currency> GetCurrency(int code)
        {
            var response = await Client.GetAsync(code.ToString());
            if(!response.IsSuccessStatusCode)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<Currency>(await response.Content.ReadAsStringAsync());
        }

        public (int Code, string Name) GetCodeByName(string name)
        {
            //TODO: лучше сюда словарь 
            var code = 0;
            if(name == "usd" || name == "доллар")
            {
                code = 145;
                name = "USD";
            }
            else if(name == "eur" || name == "евро")
            {
                code = 292;
                name = "EUR";
            }
            else if(name == "rur" || name == "рубль")
            {
                code = 298;
                name = "RUR";
            }
            else
            {
                code = 0;
                name = string.Empty;
            }

            return (code, name);
        }
    }
}