using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VkBot.Bot.Valute;
using VkBot.Data.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class BotConverter : IBotCommand
    {
        public string[] Alliases { get; set; } = { "конвертируй" };

        public async Task<string> Execute(Message msg)
        {
            var split = msg.Text.Split(' ', 3); // [команда, параметры]
            var codValute = split[1].ToLower().Trim();
            var nameValute = "";

            switch(codValute)
            {
                case "usd":
                case "доллар":
                {
                    codValute = "145";
                    nameValute = "USD";
                }
                    break;
                case "eur":
                case "евро":
                {
                    codValute = "292";
                    nameValute = "EUR";
                }
                    break;
                case "rur":
                case "рубль":
                {
                    codValute = "298";
                    nameValute = "RUR";
                }
                    break;
            }

            var request = WebRequest.Create($"http://www.nbrb.by/API/ExRates/Rates/{codValute}");

            request.Method = "GET";
            request.ContentType = "application/json";

            WebResponse response;
            try
            {
                response = await request.GetResponseAsync();
            }
            catch(Exception)
            {
                request.Abort();
                return "Я не знаю такой валюты.";
            }

            string answer;

            using(var s = response.GetResponseStream()) //читаем поток ответа
            {
                using(var reader = new StreamReader(s)) //передаем поток и считываем в answer
                {
                    answer = await reader.ReadToEndAsync();
                }
            }

            response.Close();

            var myValute = JsonConvert.DeserializeObject<ValuteConverter>(answer);

            var strBuilder = new StringBuilder();
            if(int.TryParse(split[2], out var result))
            {
                strBuilder.AppendLine($"Конвертация {nameValute} в BYN");
                strBuilder.AppendLine("_____________").AppendLine();
                strBuilder.AppendLine($"{split[2]} {nameValute} = {myValute.CurOfficialRate * result / myValute.CurScale} BYN");
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