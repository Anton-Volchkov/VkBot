using System;
using System.Net.Http;
using System.Threading.Tasks;
using CurrencyConverter.Models;
using Flurl.Http;
using Newtonsoft.Json;

namespace CurrencyConverter
{
    public class CurrencyInfo
    {
        private const string EndPoint = "https://www.nbrb.by/API/ExRates/Rates/";
        
        public async Task<Currency> GetCurrency(int code)
        {
            var response = await EndPoint.AllowAnyHttpStatus()
                                         .AppendPathSegment(code)
                                         .GetAsync();

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
            else if(name == "uah" || name == "украинский")
            {
                code = 290;
                name = "UAH";
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