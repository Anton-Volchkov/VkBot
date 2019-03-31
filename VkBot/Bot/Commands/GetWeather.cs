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
            var text = split[1].ToLower().Trim();
          

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

            var strBuilder = new StringBuilder();
            strBuilder.AppendLine($"Погода {text}");
            strBuilder.AppendLine("_____________").AppendLine();
            strBuilder.AppendLine($"Средняя температура - {oW.main.temp.ToString("0.##")}");
            strBuilder.AppendLine($"Скорость ветра - {oW.wind.speed.ToString()} м/c");
            strBuilder.AppendLine($"Направление - {oW.wind.deg.ToString()}");
            strBuilder.AppendLine($"Влажность - {oW.main.humidity.ToString()}%");
            strBuilder.AppendLine($"Давление - {((int)oW.main.pressure).ToString()}").AppendLine();
            strBuilder.AppendLine("_____________");

            return strBuilder.ToString();
        }
    }
}
