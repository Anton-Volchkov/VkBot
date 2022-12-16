using System;

namespace VkBot.Extensions;

//TODO:
public static class DateTimeExtensions
{
    public static string GetTime(this DateTime dt)
    {
        var currentSpan = new TimeSpan(dt.Hour, dt.Minute, dt.Second);
        var temp = new TimeSpan(0, 0, 0);
        var sendText = "";


        if (currentSpan > new TimeSpan(17, 40, 0))
        {
            sendText = "Учебный день закончился!";
            return sendText;
        }

        if (currentSpan <= new TimeSpan(8, 45, 0))
        {
            var ts = new TimeSpan(8, 45, 0);
            temp = ts - currentSpan;
        }
        else if (currentSpan <= new TimeSpan(9, 40, 0))
        {
            var ts = new TimeSpan(9, 40, 0);
            temp = ts - currentSpan;
        }
        else if (currentSpan <= new TimeSpan(10, 35, 0))
        {
            var ts = new TimeSpan(10, 35, 0);
            temp = ts - currentSpan;
        }
        else if (currentSpan <= new TimeSpan(11, 30, 0))
        {
            var ts = new TimeSpan(11, 30, 0);
            temp = ts - currentSpan;
        }
        else if (currentSpan <= new TimeSpan(12, 55, 0))
        {
            var ts = new TimeSpan(12, 55, 0);
            temp = ts - currentSpan;
        }
        else if (currentSpan <= new TimeSpan(13, 50, 0))
        {
            var ts = new TimeSpan(13, 50, 0);
            temp = ts - currentSpan;
        }
        else if (currentSpan <= new TimeSpan(14, 45, 0))
        {
            var ts = new TimeSpan(14, 45, 0);
            temp = ts - currentSpan;
        }
        else if (currentSpan <= new TimeSpan(15, 40, 0))
        {
            var ts = new TimeSpan(15, 40, 0);
            temp = ts - currentSpan;
        }
        else if (currentSpan <= new TimeSpan(16, 45, 0))
        {
            var ts = new TimeSpan(16, 45, 0);
            temp = ts - currentSpan;
        }
        else if (currentSpan <= new TimeSpan(17, 40, 0))
        {
            var ts = new TimeSpan(17, 40, 0);
            temp = ts - currentSpan;
        }


        if (temp > new TimeSpan(0, 45, 0))
            sendText = "До начала пары примерно - " + (temp - new TimeSpan(0, 45, 0));
        else
            sendText = "Занятие закончится примерно через - " + temp;

        return sendText;
    }
}