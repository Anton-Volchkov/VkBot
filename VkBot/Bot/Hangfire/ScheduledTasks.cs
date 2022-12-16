using Hangfire;
using OpenWeatherMap;
using Polly;
using VkBot.Domain;
using VkNet.Abstractions;
using VkNet.Model.RequestParams;

namespace VkBot.Bot.Hangfire;

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
            try
            {
                var weather = string.Empty;

                var retryPolicy = Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync(new[]
                        { TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(5) });

                await retryPolicy.ExecuteAsync(async () =>
                {
                    weather = await _weather.GetDailyWeatherAsync(group.Key);
                });

                if (string.IsNullOrWhiteSpace(weather)) continue;

                var ids = group.Where(x => x.Vk.HasValue).Select(x => x.Vk.Value).ToArray();

                foreach (var id in ids)
                    try
                    {
                        await _vkApi.Messages.SendAsync(new MessagesSendParams
                        {
                            RandomId = new DateTime().Millisecond + Guid.NewGuid().ToByteArray().Sum(x => x),
                            UserId = id,
                            Message = weather
                        });
                    }
                    catch (Exception)
                    {
                    }
            }
            catch (Exception)
            {
            }
    }

    public void InitJobs()
    {
        RecurringJob.AddOrUpdate<ScheduledTask>("SendWeather", x => x.SendWeather(),
            "5 6 * * *", TimeZoneInfo.Local);
    }
}