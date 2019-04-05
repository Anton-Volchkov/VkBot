using System.Text;
using System.Threading.Tasks;
using CurrencyConverter;
using VkBot.Data.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class Valute : IBotCommand
    {
        private readonly CurrencyInfo _currencyInfo;
        public string[] Alliases { get; set; } = { "курс" };

        public Valute(CurrencyInfo currencyInfo)
        {
            _currencyInfo = currencyInfo;
        }

        public async Task<string> Execute(Message msg)
        {
            var split = msg.Text.Split(' ', 2); // [команда, параметры]
            var name = split[1].ToLower().Trim();
            var result = _currencyInfo.GetCodeByName(name);
            if(string.IsNullOrEmpty(result.Name))
            {
                return "Я не знаю такой валюты";
            }

            var currency = await _currencyInfo.GetCurrency(result.Code);

            var strBuilder = new StringBuilder();
            strBuilder.AppendFormat("Курс {0}", currency.Abbreviation).AppendLine();
            strBuilder.AppendLine("_____________").AppendLine();
            strBuilder.AppendLine($"{currency.Scale} {currency.Abbreviation} = {currency.OfficialRate} BYN");
            strBuilder.AppendLine("_____________");

            return strBuilder.ToString();
        }
    }
}