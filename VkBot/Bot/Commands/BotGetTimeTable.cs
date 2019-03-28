﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkBot.Data.Models;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class BotGetTimeTable : IBotCommand
    {
        public  TimeTableContext _db;
        public BotGetTimeTable(TimeTableContext db)
        {
            _db = db;
        }
        public string[] Alliases { get; set; } = { "расписание" };

        public Task<string> Execute(Message msg)
        {
            return Task.Run(() =>
            {
                var SendText = _db.TimeTables.FirstOrDefault();
                if (SendText == null)
                {
                    return "Расписание пусто!";
                }
                return SendText.Timetable;
            });
        }
    }
}
