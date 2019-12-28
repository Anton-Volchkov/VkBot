using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using OpenWeatherMap;
using VkBot.Data.Models;
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
            foreach(var group in grouped)
            {
                var weather = await _weather.GetDailyWeather(group.Key);

                var ids = group.Select(x => x.Vk.Value);

                await _vkApi.Messages.SendToUserIdsAsync(new MessagesSendParams
                {
                    RandomId = new DateTime().Millisecond + Guid.NewGuid().ToByteArray().Sum(x => x),
                    UserIds = ids,
                    Message = weather
                });

                await Task.Delay(10);
            }
        }

        public async Task SendSchedule()
        {
            var day = DateTime.Now.DayOfWeek;

            if(day == DayOfWeek.Sunday || day == DayOfWeek.Saturday)
            {
                return;
            }

            var grouped = _db.GetScheduleUsers().GroupBy(x => x.Group);
            foreach(var group in grouped)
            {
                var schedule = _db.TimeTable.FirstOrDefault(x => x.Group == group.Key);

                if(schedule is null || string.IsNullOrWhiteSpace(schedule.Schedule))
                {
                    continue;
                }

                var ids = group.Select(x => x.Vk.Value).ToArray();

                await _vkApi.Messages.SendToUserIdsAsync(new MessagesSendParams
                {
                    RandomId = new DateTime().Millisecond + Guid.NewGuid().ToByteArray().Sum(x => x),
                    UserIds = ids,
                    Message = schedule.Schedule
                });

                await Task.Delay(10);
            }
        }

        public void InitJobs()
        {
            RecurringJob.AddOrUpdate<ScheduledTask>("SendWeather", x => x.SendWeather(),
                                                    "5 6 * * *", TimeZoneInfo.Local);

                //  RecurringJob.AddOrUpdate<ScheduledTask>("SendSchedule", x => x.SendSchedule(),
                //                                    "10 6 * * *", TimeZoneInfo.Local);
        }
    }
}