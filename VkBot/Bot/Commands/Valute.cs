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
    public class Valute : IBotCommand
    {
        public string[] Alliases { get; set; } = { "курс" };

        public async Task<string> Execute(Message msg)
        {
            var split = msg.Text.Split(' ', 2); // [команда, параметры]
            var valute = split[1].Trim();

            switch (valute)
            {
                case "доллар":
                    valute = "USD";
                    break;
                case "евро":
                    valute = "EUR";
                    break;
                case "рубль":
                    valute = "RUS";
                    break;
                    

                default:
                    break;
            }

            WebRequest request = WebRequest.Create($"http://www.nbrb.by/API/ExRates/Rates/{valute}?ParamMode=2");

            request.Method = "POST";
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
            strBuilder.AppendLine($"Курс {valute}");
            strBuilder.AppendLine("_____________").AppendLine();
            strBuilder.AppendLine($"1 {valute} = {MyValute.CurOfficialRate} BYN");
            strBuilder.AppendLine("_____________");

            return strBuilder.ToString();
        }
    }
}
