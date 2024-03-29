﻿using Application.Commands.Abstractions;
using OpenWeatherMap;
using VkNet.Model;

namespace Application.Commands;

public class GetWeather : IBotCommand
{
    private readonly WeatherInfo _weather;

    public GetWeather(WeatherInfo weather)
    {
        _weather = weather;
    }

    public string[] Aliases { get; set; } = { "погода" };

    public string Description { get; set; } =
        "Команда !Бот погода скажет вам текущую погоду в переданном вами городе." +
        "\nПример: !Бот погода Витебск";

    public async Task<string> ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
    {
        var split = msg.Text.Split(' ', 2); // [команда, параметры]

        if (split.Length < 2) return "Не все параметры указаны!";

        var city = split[1].Trim().ToLower();
        return await _weather.GetWeatherAsync(city);
    }
}