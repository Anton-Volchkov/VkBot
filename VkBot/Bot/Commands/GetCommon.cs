using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VkBot.Data.Abstractions;
using VkBot.Data.Models;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class GetCommon : IBotCommand
    {
        public string[] Alliases { get; set; } = { "общее" };
        private readonly MainContext _db;

        public GetCommon(MainContext db)
        {
            _db = db;
        }

        public async Task<string> Execute(Message msg)
        {
            const string scheduleEmpty = "Общего сообщения нет!";
            var sendText = await _db.Commons.FirstOrDefaultAsync();

            return sendText?.СommonInfo ?? scheduleEmpty;
        }
    }
}