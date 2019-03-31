using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class GetWeather : IBotCommand
    {
        public string[] Alliases { get; set; } = {"погода"};

        public async Task<string> Execute(Message msg)
        {
            var split = msg.Text.Split(' ', 2); // [команда, параметры]
            var text = split[1].Trim();
          

            WebRequest request = WebRequest.Create($"http://api.openweathermap.org/data/2.5/weather?q={text}&APPID=***REMOVED***");

            request.Method = "POST";
            request.ContentType = "application/json";

            WebResponse response = await request.GetResponseAsync();

            string answer = string.Empty;

            using (Stream s = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(s))
                {
                    answer = await reader.ReadToEndAsync();
                }
            }

            response.Close();

            OpenWeather.OpenWeather oW = JsonConvert.DeserializeObject<OpenWeather.OpenWeather>(answer);

            if (oW == null)
            {
                return "Город не найден";
            }

            DateTime sunrise = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            sunrise = sunrise.AddSeconds(oW.sys.sunrise).ToLocalTime();

            DateTime sunset = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            sunset = sunset.AddSeconds(oW.sys.sunset).ToLocalTime();

            var strBuilder = new StringBuilder();
            strBuilder.AppendLine($"Погода {text}");
            strBuilder.AppendLine("_____________").AppendLine();
            strBuilder.AppendLine($"Средняя температура - {oW.main.temp.ToString("0.##")}°");
            strBuilder.AppendLine($"Скорость ветра - {oW.wind.speed.ToString()} м/c");
            strBuilder.AppendLine($"Направление - {oW.wind.deg.ToString()}°");
            strBuilder.AppendLine($"Влажность - {oW.main.humidity.ToString()}%");
            strBuilder.AppendLine($"Давление - {((int)oW.main.pressure).ToString()}").AppendLine();
            strBuilder.AppendLine($"Рассвет - {sunrise.Hour}:{sunrise.Minute}");
            strBuilder.AppendLine($"Закат - {sunset.Hour}:{sunset.Minute}");
            strBuilder.AppendLine("_____________");
        
            return strBuilder.ToString();
        }
    }
}
