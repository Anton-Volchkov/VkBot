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
                var ForvaredMessages = msg.ForwardedMessages.ToArray();
               
                string text = ForvaredMessages[0].Text;
               

                var timeTable = _db.TimeTables.FirstOrDefault();
                if (timeTable != null)
                {
                    timeTable.Timetable = text;
                }
                else
                {
                    _db.TimeTables.Add(new TimeTable {
                        Timetable = text
                    });
                }

                _db.SaveChanges();

                return "Я запомнил сказанное!";
            });
        }
    }
}
