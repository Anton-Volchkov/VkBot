using System.Linq;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkBot.Data.Models;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class BotTimeTable : IBotCommand
    {
        public string[] Alliases { get; set; } = { "запомни" };
        public readonly MainContext _db;

        public BotTimeTable(MainContext db)
        {
            _db = db;
        }

        public Task<string> Execute(Message msg)
        {
            return Task.Run(() =>
            {
                var text = "";
                var ForvaredMessages = msg.ForwardedMessages;

                if(ForvaredMessages.Count > 0)
                {
                    text = ForvaredMessages[0].Text;
                }
                else
                {
                    var ReplyMessages = msg.ReplyMessage;
                    if(ReplyMessages == null)
                    {
                        return "Нет сообщения!";
                    }

                    text = ReplyMessages.Text;
                }

                var timeTable = _db.TimeTables.FirstOrDefault();
                if(timeTable != null)
                {
                    timeTable.Timetable = text;
                }
                else
                {
                    _db.TimeTables.Add(new TimeTable
                    {
                        Timetable = text
                    });
                }

                _db.SaveChanges();

                return "Я запомнил сказанное!";
            });
        }
    }
}