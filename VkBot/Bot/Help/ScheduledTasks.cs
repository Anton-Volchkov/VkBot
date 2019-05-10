using Hangfire;
using OpenWeatherMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkBot.Data.Models;
using VkNet;
using VkNet.Abstractions;

namespace VkBot.Bot.Help
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

            InitJobs();
        }
        public async Task SendWeather()
        {
            await Task.Factory.StartNew(async () =>
            {
                var grouped = _db.GetWeatherUsers().GroupBy(x => x.City);
                foreach (var group in grouped)
                {
                    var ids = group.Select(x => x.Vk).ToArray();
                    foreach (var id in ids)
                    {   ///нужен блок try потому что если у человека закрыта личка но он был подписан на рассылку, вылетает ексепшн и для других рассылка не идёт
                        try
                        {
                            await _vkApi.Messages.SendAsync(new VkNet.Model.RequestParams.MessagesSendParams
                            {
                                RandomId = new DateTime().Millisecond + Guid.NewGuid().ToByteArray().Sum(x => x),
                                UserId = id,
                                Message = await _weather.GetDailyWeather(group.Key)
                            });
                        }
                        catch(Exception)
                        {
                        }

                        await Task.Delay(100);
                    }
                }
            });
        }

        public void Dummy()
        {

        }

        private void InitJobs()
        {
            RecurringJob.AddOrUpdate<ScheduledTask>("SendWeather", x => x.SendWeather(),
                                                    "5 6 * * *", TimeZoneInfo.Local);
        }
    }
}
