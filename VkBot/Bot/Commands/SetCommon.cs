using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VkBot.Data.Abstractions;
using VkBot.Data.Models;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class SetCommon : IBotCommand
    {
        private readonly MainContext _db;

        public SetCommon(MainContext db)
        {
            _db = db;
        }

        public string[] Alliases { get; set; } = { "запомни" };

        public string Description { get; set; } =
            "Команда !Бот запомни, запоминает пересланное вам сообщение как общее сообщение для всех кто будет его запрашивать." +
            "\nПример: !Бот запомни + пересланное сообщение ";

        public async Task<string> Execute(Message msg)
        {
            var text = "";
            var forwardMessage = msg.ForwardedMessages.Count == 0 ? msg.ReplyMessage : msg.ForwardedMessages[0];

            if(forwardMessage is null)
            {
                return "Нет сообщения!";
            }

            text = forwardMessage.Text;

            var timeTable = await _db.Commons.FirstOrDefaultAsync();
            if(timeTable != null)
            {
                timeTable.СommonInfo = text;
            }
            else
            {
                await _db.Commons.AddAsync(new Common
                {
                    СommonInfo = text
                });
            }

            await _db.SaveChangesAsync();

            return "Я запомнил сказанное!";
        }
    }
}