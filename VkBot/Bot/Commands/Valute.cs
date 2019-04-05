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
    public class Valute : IBotCommand
    {
        public string[] Alliases { get; set; } = { "курс" };

        public async Task<string> Execute(Message msg)
        {
            var split = msg.Text.Split(' ', 2); // [команда, параметры]
            var Cod_Valute = split[1].ToLower().Trim();
            var NameValute = "";

            //TODO:
            switch(Cod_Valute)
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
            }

            var request = WebRequest.Create($"http://www.nbrb.by/API/ExRates/Rates/{Cod_Valute}");

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

            var MyValute = JsonConvert.DeserializeObject<ValuteConverter>(answer);


            var strBuilder = new StringBuilder();
            strBuilder.AppendLine($"Курс {NameValute}");
            strBuilder.AppendLine("_____________").AppendLine();
            strBuilder.AppendLine($"{MyValute.CurScale} {NameValute} = {MyValute.CurOfficialRate} BYN");
            strBuilder.AppendLine("_____________");

            return strBuilder.ToString();
        }
    }
}