using Hangfire;
using OpenWeatherMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkBot.Data.Models;
using VkNet;

namespace VkBot.Bot.Help
{
    public class ScheduledTask
    {
        private readonly MainContext _db;
        private readonly VkApi _vkApi;
        private readonly WeatherInfo _weather;
        public ScheduledTask(MainContext db, VkApi vkApi, WeatherInfo weather)
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
                    {
                        await _vkApi.Messages.SendAsync(new VkNet.Model.RequestParams.MessagesSendParams
                        {
                            RandomId = new DateTime().Millisecond + Guid.NewGuid().ToByteArray().Sum(x => x),
                            UserId = id,
                            Message = await _weather.GetWeather(group.Key)
                        });
                    }
                    await Task.Delay(300);
                }
            });
        }

        public void Dummy()
        {

        }

        private void InitJobs()
        {
            RecurringJob.AddOrUpdate<ScheduledTask>("SendWeather", x => x.SendWeather(),
                                                    "51 16 * * *", TimeZoneInfo.Local);
        }
    }
}
