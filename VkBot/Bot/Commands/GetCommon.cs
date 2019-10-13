using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VkBot.Data.Abstractions;
using VkBot.Data.Models;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class GetCommon : IBotCommand
    {
        private readonly MainContext _db;

        public GetCommon(MainContext db)
        {
            _db = db;
        }

        public string[] Aliases { get; set; } = { "общее" };

        public string Description { get; set; } =
            "Команда !Бот общее вернёт вам общее для все сообщение, которое было установлено при помощи команды(!Бот общее)." +
            "\nПример: !Бот общее";

        public async Task<string> Execute(Message msg)
        {
            const string scheduleEmpty = "Общего сообщения нет!";
            var sendText = await _db.Commons.FirstOrDefaultAsync();

            return sendText?.СommonInfo ?? scheduleEmpty;
        }
    }
}