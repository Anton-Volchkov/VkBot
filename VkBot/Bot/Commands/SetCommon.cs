using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VkBot.Data.Abstractions;
using VkBot.Data.Models;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class SetCommon : IBotCommand
    {
        public string[] Alliases { get; set; } = { "запомни" };
        private readonly MainContext _db;

        public SetCommon(MainContext db)
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

            var timeTable = await _db.Commons.FirstOrDefaultAsync();
            if(timeTable != null)
            {
                timeTable.commonInfo = text;
            }
            else
            {
                await _db.Commons.AddAsync(new Data.Models.Common
                {
                   commonInfo = text
                });
            }

            await _db.SaveChangesAsync();

            return "Я запомнил сказанное!";
        }
    }
}