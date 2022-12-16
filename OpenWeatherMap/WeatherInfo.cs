using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenWeatherMap.Models.Daily;

namespace OpenWeatherMap
{
    public class WeatherInfo
    {
        private const string EndPoint = "https://api.openweathermap.org/data/2.5/";
        private const string Lang = "ru";

        private const double PressureConvert = 0.75006375541921;
        private readonly string Token;

        public WeatherInfo(string token)
        {
            if(string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token), "Токен отсутствует");
            }

            Token = token;
        }

        public async Task<string> GetWeatherAsync(string city)
        {
            city = char.ToUpper(city[0]) + city.Substring(1); //TODO ?

            var response = await GetCurrentWeatherResponseAsync(city);

            if(!response.ResponseMessage.IsSuccessStatusCode)
            {
                var newCity = city.Replace("е", "ё");

                response = await GetCurrentWeatherResponseAsync(newCity);

                if(!response.ResponseMessage.IsSuccessStatusCode)
                {
                    return $"Город {city} не найден.";
                }

                city = newCity;
            }

            var w = JsonConvert.DeserializeObject<Models.WeatherInfo>(await response.GetStringAsync());

            var strBuilder = new StringBuilder();
            strBuilder.AppendFormat("Погода {0} на данный момент", city).AppendLine();
            strBuilder.AppendLine("_____________").AppendLine();
            strBuilder.AppendFormat("Средняя температура: {0:N0}°С", w.Weather.Temperature).AppendLine();
            strBuilder.AppendFormat("Описание погоды: {0}", w.Info[0].State).AppendLine();
            strBuilder.AppendFormat("Скорость ветра: {0:N0} м/с", w.Wind.Speed).AppendLine();
            strBuilder.AppendFormat("Направление: {0}", w.Wind.Degrees).AppendLine();
            strBuilder.AppendFormat("Влажность: {0}%", w.Weather.Humidity).AppendLine();
            strBuilder.AppendFormat("Давление: {0:N0} мм.рт.ст", w.Weather.Pressure * PressureConvert).AppendLine();
            strBuilder.AppendFormat("Рассвет в {0:HH:mm}", UnixToDateTime(w.OtherInfo.Sunrise)).AppendLine();
            strBuilder.AppendFormat("Закат в {0:HH:mm}", UnixToDateTime(w.OtherInfo.Sunset)).AppendLine();
            strBuilder.AppendLine("_____________");

            return strBuilder.ToString();
        }

        public async Task<string> GetDailyWeatherAsync(string city)
        {
            city = char.ToUpper(city[0]) + city.Substring(1); //TODO ?
            var date = DateTime.Now;
            
            var response = await GetDailyWeatherResponseAsync(city);

            if(!response.ResponseMessage.IsSuccessStatusCode)
            {
                var newCity = city.Replace("е", "ё");
                response = await GetDailyWeatherResponseAsync(city);

                if(!response.ResponseMessage.IsSuccessStatusCode)
                {
                    return $"Город {city} не найден. Проверьте введённые данные.";
                }

                city = newCity;
            }

            var weatherToday = JsonConvert.DeserializeObject<DailyWeather>(await response.GetStringAsync()).Daily.First();

            if(weatherToday is null)
            {
                return $"Погода на {date:dd.MM} не найдена";
            }

            var strBuilder = new StringBuilder();
            
            strBuilder.AppendFormat("Погода в городе {0} на сегодня ({1:dddd, d MMMM}):", city, date).AppendLine();
            strBuilder.AppendLine("_____________").AppendLine();
            strBuilder.AppendFormat("Температура: от {0:+#;-#;0}°С до {1:+#;-#;0}°С", weatherToday.Temp.Min, weatherToday.Temp.Max).AppendLine();
            strBuilder.AppendFormat("Температура утром: {0:+#;-#;0}°С", weatherToday.Temp.Morn).AppendLine(); 
            strBuilder.AppendFormat("Температура днем: {0:+#;-#;0}°С", weatherToday.Temp.Day).AppendLine();
            strBuilder.AppendFormat("Температура вечером: {0:+#;-#;0}°С", weatherToday.Temp.Eve).AppendLine();
            strBuilder.AppendFormat("Температура ночью: {0:+#;-#;0}°С", weatherToday.Temp.Night).AppendLine();
            strBuilder.AppendFormat("Описание погоды: {0}", weatherToday.Weather[0].Description).AppendLine();
            strBuilder.AppendFormat("Влажность: {0}%", weatherToday.Humidity).AppendLine();
            strBuilder.AppendFormat("Ветер: {0:N0} м/с", weatherToday.WindSpeed).AppendLine();
            strBuilder.AppendFormat("Давление: {0:N0} мм.рт.ст", weatherToday.Pressure * PressureConvert)
                          .AppendLine();
            strBuilder.AppendFormat("Облачность: {0}%", weatherToday.Clouds).AppendLine().AppendLine();
            strBuilder.AppendLine("_____________");

            return strBuilder.ToString();
        }

        private async Task<IFlurlResponse> GetCurrentWeatherResponseAsync(string city)
        {
            return await BuildRequest()
                         .AppendPathSegment("weather")
                         .SetQueryParam("q", city)
                         .GetAsync();
        }

        private async Task<IFlurlResponse> GetDailyWeatherResponseAsync(string city)
        {
            var coordinatesByCity = await GetСoordinatesByCityAsync(city);

            return await BuildRequest()
                         .AppendPathSegment("onecall")
                         .SetQueryParam("lat", coordinatesByCity.lat)
                         .SetQueryParam("lon", coordinatesByCity.lon)
                         .SetQueryParam("exclude","hourly,current")
                         .GetAsync();
        }

        private IFlurlRequest BuildRequest()
        {
            return EndPoint.AllowAnyHttpStatus()
                           .SetQueryParam("units", "metric")
                           .SetQueryParam("appid", Token)
                           .SetQueryParam("lang", Lang);
        }

        private async Task<(string lat, string lon)> GetСoordinatesByCityAsync(string city)
        {
            var response = await "https://geocode.xyz/"
                      .AllowAnyHttpStatus()
                      .AppendPathSegment(city)
                      .SetQueryParam("json", "1")
                      .GetAsync();
            
            if (!response.ResponseMessage.IsSuccessStatusCode)
            {
                throw new ArgumentException("Погоды по данному городу не найдено!");
            }

            var jo = JObject.Parse(await response.GetStringAsync());

            var latt = jo.Properties()?.FirstOrDefault(x => x.Name == "latt")?.Value.ToString();
            var lon = jo.Properties()?.FirstOrDefault(x => x.Name == "longt")?.Value.ToString();

            return (lat: latt, lon: lon);
        }

        internal DateTime UnixToDateTime(double unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}