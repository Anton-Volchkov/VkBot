using System.Threading;
using System.Threading.Tasks;
using CurrencyConverter.Models;
using Flurl.Http;
using Newtonsoft.Json;

namespace CurrencyConverter;

public class CurrencyInfo
{
    private const string EndPoint = "https://www.nbrb.by/API/ExRates/Rates/";

    public async Task<Currency> GetCurrencyAsync(int code, CancellationToken cancellationToken = default)
    {
        var response = await EndPoint.AllowAnyHttpStatus()
            .AppendPathSegment(code)
            .GetAsync(cancellationToken: cancellationToken);

        return !response.ResponseMessage.IsSuccessStatusCode ? null : JsonConvert.DeserializeObject<Currency>(await response.GetStringAsync());
    }

    public (int Code, string Name) GetCodeByName(string name)
    {
        //TODO: лучше сюда словарь 
        var code = 0;
        switch (name)
        {
            case "usd":
            case "доллар":
                code = 145;
                name = "USD";
                break;
            case "eur":
            case "евро":
                code = 292;
                name = "EUR";
                break;
            case "rur":
            case "рубль":
                code = 298;
                name = "RUR";
                break;
            case "uah":
            case "украинский":
                code = 290;
                name = "UAH";
                break;
            default:
                code = 0;
                name = string.Empty;
                break;
        }

        return (code, name);
    }
}