using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VkBot.Data.Abstractions;
using VkBot.Data.Models;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class GetTimeTable : IBotCommand
    {
        public string[] Alliases { get; set; } = { "расписание" };
        private readonly MainContext _db;

        public GetTimeTable(MainContext db)
        {
            _db = db;
        }

        public async Task<string> Execute(Message msg)
        {
            const string scheduleEmpty = "Расписание пустое!";
            var sendText = await _db.TimeTables.FirstOrDefaultAsync();

            return sendText?.Timetable ?? scheduleEmpty;
        }
    }
}