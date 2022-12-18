using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoronaVirus.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using VkBot.Domain;
using VkBot.Domain.Models;
using YandexTranslator;

namespace CoronaVirus;

public class CoronaInfo
{
    private readonly MainContext _db;
    private readonly Translator _translator;

    public CoronaInfo(Translator translator, MainContext dbContext)
    {
        _translator = translator;
        _db = dbContext;
    }

    public async Task<string> GetCoronaVirusInfoAsync(string country, CancellationToken cancellationToken = default)
    {
        string countryInEnglish; 

        var countryData = await _db.Countries.FirstOrDefaultAsync(x => x.RussianName == country.ToLower(), cancellationToken: cancellationToken);

        if (countryData is null)
            countryInEnglish = await _translator.Translate(country, "ru-en");
        else
            countryInEnglish = countryData.EnglishName;

        if (countryData is null)
        {
            await _db.Countries.AddAsync(new Country
            {
                RussianName = country.ToLower(),
                EnglishName = countryInEnglish
            }, cancellationToken);

            await _db.SaveChangesAsync(cancellationToken);
        }

        var url = "https://covid-api.com/api/";

        var client = new HttpClient
        {
            BaseAddress = new Uri(url)
        };

        var response = await client.GetAsync($"reports?region_name={countryInEnglish}", cancellationToken);

        if (!response.IsSuccessStatusCode) return "Информации по COVID-19 не найдено!";

        var covidInfo =
            JsonConvert.DeserializeObject<CovidDTO>(await response.Content.ReadAsStringAsync(cancellationToken));

        if(!covidInfo?.Data?.Any() ?? true)
        {
            return "Информации по COVID-19 не найдено!";
        }

        var sb = new StringBuilder();
        sb.AppendLine("Статистика по COVID-19").AppendLine();
        sb.AppendLine("________________").AppendLine();


        sb.AppendLine($"Страна: {country.ToUpper()}").AppendLine();
        sb.AppendLine($"Всего было заражено: {covidInfo.Data.Sum(x => x.Confirmed)}").AppendLine();
        sb.AppendLine($"Смертей: {covidInfo.Data.Sum(x => x.Deaths)}").AppendLine();

        sb.AppendLine("________________").AppendLine();

        return sb.ToString();
    }
}