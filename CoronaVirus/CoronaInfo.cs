using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CoronaVirus.Models;
using Newtonsoft.Json;
using YandexTranslator;

namespace CoronaVirus
{
    public class CoronaInfo
    {
        private readonly Translator _translator;
        public CoronaInfo(Translator translator)
        {
            _translator = translator;
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
                var countries = JsonConvert.DeserializeObject<List<CountryInfo>>(await response.Content.ReadAsStringAsync());
                var countryOnEnglish = await _translator.Translate(country, "ru-en");

                var needCountry =
                     countries.FirstOrDefault(x => x.Country.Equals(countryOnEnglish,
                                                                         StringComparison.CurrentCultureIgnoreCase));

                if(needCountry is null)
                {
                    return "Информации по COVID-19 в этой стране не найдено!";
                }

                sb.AppendLine($"Страна: {country.ToUpper()}").AppendLine();
                sb.AppendLine($"Всего было заражено: {needCountry.Cases}").AppendLine();
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
