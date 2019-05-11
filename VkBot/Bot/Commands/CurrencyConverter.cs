using System.Text;
using System.Threading.Tasks;
using CurrencyConverter;
using VkBot.Data.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class CurrencyConverter : IBotCommand
    {
        private readonly CurrencyInfo _currencyInfo;
        public string[] Alliases { get; set; } = { "конвертируй" };

        public CurrencyConverter(CurrencyInfo currencyInfo)
        {
            _currencyInfo = currencyInfo;
        }

        public async Task<string> Execute(Message msg)
        {
            var split = msg.Text.Split(' ', 3); // [команда, параметры]
            if(!int.TryParse(split[2], out var inputMoney))
            {
                return "Конвертация не удалась.";
            }

            var name = split[1].ToLower().Trim();
            var result = _currencyInfo.GetCodeByName(name);
            if(string.IsNullOrEmpty(result.Name))
            {
                return "Я не знаю такой валюты";
            }

            var currency = await _currencyInfo.GetCurrency(result.Code);

            var strBuilder = new StringBuilder();
            strBuilder.AppendFormat("Конвертация {0} в BYN по курсу НБ РБ", currency.Abbreviation).AppendLine();
            strBuilder.AppendLine("_____________").AppendLine();
            strBuilder.AppendFormat("{0} {1} = {2} BYN",
                                    split[2], currency.Abbreviation, currency.OfficialRate * inputMoney / currency.Scale)
                      .AppendLine();
            strBuilder.AppendLine("_____________");

            return strBuilder.ToString();
        }
    }
}