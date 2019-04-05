using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VkBot.Data.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class GetWeather : IBotCommand
    {
        public string[] Alliases { get; set; } = { "погода" };

        public async Task<string> Execute(Message msg)
        {
            var split = msg.Text.Split(' ', 2); // [команда, параметры]
            var city = split[1].Trim();


            var request =
                WebRequest.Create($"http://api.openweathermap.org/data/2.5/weather?q={city}&APPID=***REMOVED***&lang=ru&units=metric");

            request.Method = "POST";
            request.ContentType = "application/json";

            WebResponse response;
            try
            {
                response = await request.GetResponseAsync();
            }
            catch(Exception)
            {
                request.Abort();
                return $"Город {city} не найден.";
            }

            var answer = string.Empty;

            using(var s = response.GetResponseStream()) //читаем поток ответа
            {
                using(var reader = new StreamReader(s)) //передаем поток и считываем в answer
                {
                    answer = await reader.ReadToEndAsync();
                }
            }

            response.Close();

            OpenWeather.OpenWeather oW = oW = JsonConvert.DeserializeObject<OpenWeather.OpenWeather>(answer);


            var sunrise = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            sunrise = sunrise.AddSeconds(oW.sys.sunrise).ToLocalTime();

            var sunset = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            sunset = sunset.AddSeconds(oW.sys.sunset).ToLocalTime();

            var strBuilder = new StringBuilder();
            strBuilder.AppendLine($"Погода {city}");
            strBuilder.AppendLine("_____________").AppendLine();
            strBuilder.AppendLine($"Средняя температура:  {oW.main.temp:0.##}°С");
            strBuilder.AppendLine($"Описание погоды:  {oW.weather[0].description}");
            strBuilder.AppendLine($"Скорость ветра:  {oW.wind.speed} м/c");
            strBuilder.AppendLine($"Направление:  {oW.wind.deg}°");
            strBuilder.AppendLine($"Влажность: {oW.main.humidity}%");
            strBuilder.AppendLine($"Давление:  {((int)oW.main.pressure).ToString()}мм").AppendLine();
            strBuilder.AppendLine($"Рассвет -  {sunrise.Hour}:{sunrise.Minute}");
            strBuilder.AppendLine($"Закат -  {sunset.Hour}:{sunset.Minute}");
            strBuilder.AppendLine("_____________");

            return strBuilder.ToString();
        }
    }
}