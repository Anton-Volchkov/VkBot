using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkBot.Data.Models;
using VkNet.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class CheckOnlineUser : IBotCommand
    {
        private readonly MainContext _db;
        private readonly IVkApi _vkApi;
        public string[] Aliases { get; set; } = { "онлайн" };

        public string Description { get; set; } =
            "Команда !Бот онлайн скажет вам кто онлайн в беседе.\nПример: !Бот онлайн";

        public CheckOnlineUser(MainContext db, IVkApi api)
        {
            _db = db;
            _vkApi = api;
        }

        public Task<string> Execute(Message msg)
        {
            if(msg.PeerId.Value == msg.FromId.Value)
            {
                return Task.FromResult("Команда работает только в групповых чатах!");
            }

            var strBuilder = new StringBuilder();

            strBuilder.AppendLine("Пользователи чата онлайн");
            strBuilder.AppendLine("_____________").AppendLine();

            var chat = _vkApi.Messages.GetConversationMembers(msg.PeerId.Value, new[] { "online" })
                             .Profiles
                             .Where(x => x.Online.HasValue).ToArray();

            foreach(var user in chat)
            {
                if(user.Online.HasValue && user.Online.Value)
                {
                    if(user.OnlineMobile.HasValue)
                    {
                        strBuilder.AppendLine($"{user.FirstName} {user.LastName} (📱)");
                    }
                    else
                    {
                        strBuilder.AppendLine($"{user.FirstName} {user.LastName} (💻)");
                    }
                }
            }

            strBuilder.AppendLine("_____________").AppendLine();

            return Task.FromResult(strBuilder.ToString());
        }
    }
}