using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkBot.Data.Models;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class BotTimeTable:IBotCommand
    {
        public readonly TimeTableContext _db;
        public BotTimeTable(TimeTableContext db)
        {
            _db = db;
        }
        public string[] Alliases { get; set; } = { "запомни" };

        public Task<string> Execute(Message msg)
        {
            return Task.Run(() =>
            {
                string text = msg.Body;
                text = text.Substring(text.IndexOf("[") + 1, text.IndexOf(']') - text.IndexOf('[') - 1);
                _db.TimeTables.Add(new TimeTable
                {
                    Timetable = text
                });
                _db.SaveChanges();
                return "Я запомнил сказанное!";
            });
        }
    }
}
