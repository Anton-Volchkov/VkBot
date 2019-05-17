using System.Threading.Tasks;
using OpenWeatherMap;
using VkBot.Data.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class GetWeather : IBotCommand
    {
        public string[] Alliases { get; set; } = { "погода" };
        public string Description { get; set; } = "Команда !Бот погода скажет вам текущую погоду в переданном вами городе." +
                                                  "\nПример: !Бот погода Витебск";

        private readonly WeatherInfo _weather;

        public GetWeather(WeatherInfo weather)
        {
            _weather = weather;
        }

        public async Task<string> Execute(Message msg)
        {
            var split = msg.Text.Split(' ', 2); // [команда, параметры]
            var city = split[1].Trim().ToLower();
            return await _weather.GetWeather(city);
        }
    }
}