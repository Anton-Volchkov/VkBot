using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using OpenWeatherMap;
using VkBot.Data.Models;
using VkNet.Abstractions;
using VkNet.Model.RequestParams;

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
                foreach(var group in grouped)
                {
                    var ids = group.Select(x => x.Vk).ToArray();
                    foreach(var id in ids)
                    {
                        //нужен блок try потому что если у человека закрыта личка но он был подписан на рассылку, вылетает ексепшн и для других рассылка не идёт
                        try
                        {
                            await _vkApi.Messages.SendAsync(new MessagesSendParams
                            {
                                RandomId = new DateTime().Millisecond + Guid.NewGuid().ToByteArray().Sum(x => x),
                                UserId = id,
                                Message = await _weather.GetDailyWeather(group.Key)
                            });
                        }
                        catch(Exception) { }

                        await Task.Delay(60);
                    }
                }
            });
        }

        public async Task SendSchedule()
        {
            await Task.Factory.StartNew(async () =>
            {
                var grouped = _db.GetScheduleUsers().GroupBy(x => x.Group);
                foreach(var group in grouped)
                {
                   
                    var schedule =  _db.TimeTable.FirstOrDefault(x => x.Group == group.Key);

                    if(string.IsNullOrWhiteSpace(schedule.Schedule))
                    {
                        continue;
                    }

                    var ids = group.Select(x => x.Vk).ToArray();
                    foreach(var id in ids)
                    {
                        //нужен блок try потому что если у человека закрыта личка но он был подписан на рассылку, вылетает ексепшн и для других рассылка не идёт
                        try
                        {
                            await _vkApi.Messages.SendAsync(new MessagesSendParams
                            {
                                RandomId = new DateTime().Millisecond + Guid.NewGuid().ToByteArray().Sum(x => x),
                                UserId = id,
                                Message = schedule.Schedule
                            });
                        }
                        catch(Exception) { }

                        await Task.Delay(60);
                    }
                }
            });
        }

        public void Dummy() { }

        private void InitJobs()
        {
            RecurringJob.AddOrUpdate<ScheduledTask>("SendWeather", x => x.SendWeather(),
                                                    "5 6 * * *", TimeZoneInfo.Local);

            RecurringJob.AddOrUpdate<ScheduledTask>("SendSchedule", x => x.SendSchedule(),
                                                    "57 8 * * *", TimeZoneInfo.Local);
        }
    }
}