﻿using Application.Commands.Abstractions;
using VkBot.Extensions;
using VkNet.Model;

namespace Application.Commands;

public class Bell : IBotCommand
{
    public string[] Aliases { get; set; } = { "звонок" };

    public string Description { get; set; } =
        "Команда !Бот звонок скажет вам время до окнчания пары/полупары или перемены.\nПример: !Бот звонок";

    public Task<string> ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
    {
        var bells = new[]
        {
            "1) 8:00 - 8:45 / 8:55 - 9:40", "2) 9:50 - 10:35 / 10:45 - 11:30",
            "3)12:10 - 12:55 / 13:05 - 13:50", "4) 14:00 - 14:45 / 14:55 - 15:40",
            "5) 16:00 - 16:45 / 16:55 - 17:40"
        };
        var dt = DateTime.Now;
        return Task.FromResult($"Сейчас - {dt.ToShortTimeString()}\n\n" +
                               $"Расписание звонков: \n {string.Join('\n', bells)}\n{dt.GetTime()}");
    }
}