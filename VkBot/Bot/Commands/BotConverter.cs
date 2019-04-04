using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VkBot.Bot.Valute;
using VkBot.Data.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class BotConverter : IBotCommand
    {
        public string[] Alliases { get; set; } = {"конвертируй"};

        public async Task<string> Execute(Message msg)
        {
            var split = msg.Text.Split(' ', 3); // [команда, параметры]
            var Cod_Valute = split[1].ToLower().Trim();
            string NameValute = "";

            switch (Cod_Valute)
            {
                case "usd":
                case "доллар":
                    {
                        Cod_Valute = "145";
                        NameValute = "USD";
                    }
                    break;
                case "eur":
                case "евро":
                    {
                        Cod_Valute = "292";
                        NameValute = "EUR";
                    }
                    break;
                case "rur":
                case "рубль":
                    {
                        Cod_Valute = "298";
                        NameValute = "RUR";
                    }
                    break;


                default:
                    break;
            }

            WebRequest request = WebRequest.Create($"http://www.nbrb.by/API/ExRates/Rates/{Cod_Valute}");

            request.Method = "GET";
            request.ContentType = "application/json";

            WebResponse response;
            try
            {
                response = await request.GetResponseAsync();
            }
            catch (Exception)
            {
                request.Abort();
                return $"Я не знаю такой валюты.";
            }

            string answer = string.Empty;

            using (Stream s = response.GetResponseStream())//читаем поток ответа
            {
                using (StreamReader reader = new StreamReader(s)) //передаем поток и считываем в answer
                {
                    answer = await reader.ReadToEndAsync();
                }
            }
            response.Close();

            ValuteConverter MyValute = JsonConvert.DeserializeObject<ValuteConverter>(answer);

            var strBuilder = new StringBuilder();
            if (Int32.TryParse(split[2], out int result))
            {
                strBuilder.AppendLine($"Конвертация {NameValute} в BYN");
                strBuilder.AppendLine("_____________").AppendLine();
                strBuilder.AppendLine($"{split[2]} {NameValute} = {MyValute.CurOfficialRate * result / MyValute.CurScale} BYN");
                strBuilder.AppendLine("_____________");
            }
            else
            {
                strBuilder.AppendLine("Конвертация не удалась.");
            }

           
           

            return strBuilder.ToString();
        }
    }
}
