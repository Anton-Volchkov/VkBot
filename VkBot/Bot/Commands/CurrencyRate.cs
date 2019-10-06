using System.Text;
using System.Threading.Tasks;
using CurrencyConverter;
using VkBot.Data.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class CurrencyRate : IBotCommand
    {
        private readonly CurrencyInfo _currencyInfo;

        public CurrencyRate(CurrencyInfo currencyInfo)
        {
            _currencyInfo = currencyInfo;
        }

        public string[] Alliases { get; set; } = { "курс" };

        public string Description { get; set; } =
            "Команда !Бот курс вернёт курс переданной вам валюты(По курсу НБ РБ)." +
            "\nПример: !Бот курс USD";

        public async Task<string> Execute(Message msg)
        {
            var split = msg.Text.Split(' ', 2); // [команда, параметры]

            if(split.Length < 2)
            {
                return "Не все параметры указаны!";
            }

            var name = split[1].ToLower().Trim();
            var result = _currencyInfo.GetCodeByName(name);
            if(string.IsNullOrEmpty(result.Name))
            {
                return "Я не знаю такой валюты";
            }

            var currency = await _currencyInfo.GetCurrency(result.Code);

            var strBuilder = new StringBuilder();
            strBuilder.AppendFormat("Курс {0} НБ РБ", currency.Abbreviation).AppendLine();
            strBuilder.AppendLine("_____________").AppendLine();
            strBuilder.AppendLine($"{currency.Scale} {currency.Abbreviation} = {currency.OfficialRate} BYN");
            strBuilder.AppendLine("_____________");

            return strBuilder.ToString();
        }
    }
}