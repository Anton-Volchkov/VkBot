using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using OpenWeatherMap;
using VkBot.Domain;
using VkNet.Abstractions;
using VkNet.Model.RequestParams;

namespace VkBot.Bot.Hangfire
{
    public class ScheduledTask
    {
        private readonly MainContext _db;
        private readonly IVkApi _vkApi;
        private readonly WeatherInfo _weather;

        public ScheduledTask(MainContext db, IVkApi vkApi, WeatherInfo weather)
        {
            _db = db;
            _vkApi = vkApi;
            _weather = weather;
        }

        public async Task SendWeather()
        {
            var grouped = _db.GetWeatherUsers().GroupBy(x => x.City);
            foreach (var group in grouped)
            {
                try
                {
                    string weather = await _weather.GetDailyWeatherAsync(group.Key);

                    if (string.IsNullOrWhiteSpace(weather))
                    {
                        continue;
                    }

                    var ids = group.Where(x => x.Vk.HasValue).Select(x => x.Vk.Value).ToArray();

                    foreach (var id in ids)
                    {
                        try
                        {
                            await _vkApi.Messages.SendAsync(new MessagesSendParams
                            {
                                RandomId = new DateTime().Millisecond + Guid.NewGuid().ToByteArray().Sum(x => x),
                                UserId = id,
                                Message = weather
                            });

                            throw new Exception();
                        }
                        catch (Exception)
                        {
                            continue;
                        }

                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        public void InitJobs()
        {
            RecurringJob.AddOrUpdate<ScheduledTask>("SendWeather", x => x.SendWeather(),
                                                    "5 6 * * *", TimeZoneInfo.Local);
        }
    }
}