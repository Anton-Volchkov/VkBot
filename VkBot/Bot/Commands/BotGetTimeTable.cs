using System.Linq;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkBot.Data.Models;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class BotGetTimeTable : IBotCommand
    {
        public string[] Alliases { get; set; } = { "расписание" };
        public readonly MainContext _db;

        public BotGetTimeTable(MainContext db)
        {
            _db = db;
        }

        public Task<string> Execute(Message msg)
        {
            return Task.Run(() =>
            {
                var SendText = _db.TimeTables.FirstOrDefault();
                if(SendText == null)
                {
                    return "Расписание пустое!";
                }

                return SendText.Timetable;
            });
        }
    }
}