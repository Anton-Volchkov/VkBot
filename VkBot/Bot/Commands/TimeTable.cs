using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VkBot.Data.Abstractions;
using VkBot.Data.Models;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class TimeTable : IBotCommand
    {
        public string[] Alliases { get; set; } = { "общее" };
        private readonly MainContext _db;

        public TimeTable(MainContext db)
        {
            _db = db;
        }

        public async Task<string> Execute(Message msg)
        {
            var text = "";
            var forwardMessage = msg.ForwardedMessages.Count == 0 ?
                                     msg.ReplyMessage :
                                     msg.ForwardedMessages[0];

            if(forwardMessage is null)
            {
                return "Нет сообщения!";
            }

            text = forwardMessage.Text;

            var timeTable = await _db.TimeTables.FirstOrDefaultAsync();
            if(timeTable != null)
            {
                timeTable.Timetable = text;
            }
            else
            {
                await _db.TimeTables.AddAsync(new Data.Models.TimeTable
                {
                    Timetable = text
                });
            }

            await _db.SaveChangesAsync();

            return "Я запомнил сказанное!";
        }
    }
}