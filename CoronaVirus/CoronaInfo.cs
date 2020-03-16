using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CoronaVirus.Models;
using HtmlAgilityPack;
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
        public async Task<string> GetCoronaVIrusInfo(string country = "")
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
            sb.AppendLine("информация по COVID-19").AppendLine();
            sb.AppendLine("________________").AppendLine();

            if(string.IsNullOrWhiteSpace(country))
            {
                var answer = JsonConvert.DeserializeObject<FullInfo>(await response.Content.ReadAsStringAsync());

                sb.AppendLine($"Зараженных: {answer.Cases}").AppendLine();
                sb.AppendLine($"Смертей: {answer.Deaths}").AppendLine();
                sb.AppendLine($"Вылечено: {answer.Recovered}");
            }
            else
            {
                var countries = JsonConvert.DeserializeObject<List<CityInfo>>(await response.Content.ReadAsStringAsync());
                var countryOnEnglish = await _translator.Translate(country, "ru-en");

                var needCountry =
                     countries.FirstOrDefault(x => x.Country.Equals(countryOnEnglish,
                                                                         StringComparison.CurrentCultureIgnoreCase));

                if(needCountry is null)
                {
                    return "Информации по COVID-19 в этой стране не найдено!";
                }

                sb.AppendLine($"Страна: {country}").AppendLine();
                sb.AppendLine($"Заражено: {needCountry.Cases}").AppendLine();
                sb.AppendLine($"Смертей: {needCountry.Deaths}").AppendLine();
                sb.AppendLine($"Вылечено: {needCountry.Recovered}").AppendLine();
              
            }

            sb.AppendLine("________________").AppendLine();

            return sb.ToString();
        }
      
    }
}
