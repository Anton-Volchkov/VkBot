using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CoronaVirus.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using VkBot.Domain;
using VkBot.Domain.Models;
using YandexTranslator;

namespace CoronaVirus
{
    public class CoronaInfo
    {
        private readonly Translator _translator;
        private readonly MainContext _db;
        public CoronaInfo(Translator translator, MainContext dbContext)
        {
            _translator = translator;
            _db = dbContext;
        }
        public async Task<string> GetCoronaVirusInfo(string country = "")
        {
            string url = "https://coronavirus-19-api.herokuapp.com/";

            HttpClient Client = new HttpClient()
            {
                BaseAddress = new Uri(url)
            };

            var response = await Client.GetAsync(string.IsNullOrWhiteSpace(country)? url + "all" : url + "countries");

            if (!response.IsSuccessStatusCode)
            {
                return "Информации по COVID-19 не найдено!";
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Статистика по COVID-19").AppendLine();
            sb.AppendLine("________________").AppendLine();

            if(string.IsNullOrWhiteSpace(country))
            {
                var answer = JsonConvert.DeserializeObject<FullInfo>(await response.Content.ReadAsStringAsync());

                sb.AppendLine($"Всего было заражено: {answer.Cases}").AppendLine();
                sb.AppendLine($"Зараженных сейчас: {answer.Cases - answer.Recovered}").AppendLine();
                sb.AppendLine($"Вылечено: {answer.Recovered}").AppendLine();
                sb.AppendLine($"Смертей: {answer.Deaths}").AppendLine();
            }
            else
            {
                string countryOnEnglish = string.Empty;
                var countries = JsonConvert.DeserializeObject<List<CountryInfo>>(await response.Content.ReadAsStringAsync());

                var countryData =  await _db.Countries.FirstOrDefaultAsync(x => x.RussianName == country.ToLower());

                if(countryData is null)
                {
                    countryOnEnglish = await _translator.Translate(country, "ru-en");
                }
                else
                {
                    countryOnEnglish = countryData.EnglishName;
                }
                 

                var needCountry =
                     countries.FirstOrDefault(x => x.Country.Equals(countryOnEnglish,
                                                                         StringComparison.CurrentCultureIgnoreCase));

                if(needCountry is null)
                {
                    return "Информации по COVID-19 в этой стране не найдено!";
                }

                if(countryData is null)
                {
                    await _db.Countries.AddAsync(new Country()
                    {
                        RussianName = country.ToLower(),
                        EnglishName = countryOnEnglish
                    });

                    await _db.SaveChangesAsync();
                }

                sb.AppendLine($"Страна: {country.ToUpper()}").AppendLine();
                sb.AppendLine($"Всего было заражено: {needCountry.Cases}").AppendLine();
                sb.AppendLine($"Заражено сегодня: {needCountry.TodayCases}").AppendLine();
                sb.AppendLine($"Заражено на текущий момент: {needCountry.Active}").AppendLine();
                sb.AppendLine($"Всего проведено тестов: {needCountry.TotalTests}").AppendLine();
                sb.AppendLine($"Вылечено: {needCountry.Recovered}").AppendLine();
                sb.AppendLine($"В критическом состоянии: {needCountry.Critical}").AppendLine();
                sb.AppendLine($"Смертей: {needCountry.Deaths}").AppendLine();

            }

            sb.AppendLine("________________").AppendLine();

            return sb.ToString();
        }
      
    }
}
