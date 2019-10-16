using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkBot.Data.Models;
using VkNet.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class GetTopUsersInChat : IBotCommand
    {
        private readonly MainContext _db;
        private readonly IVkApi _vkApi;
        private readonly RolesHandler _checker;
        public string[] Aliases { get; set; } = { "топ" };
        public string Description { get; set; }

        public GetTopUsersInChat(MainContext db, IVkApi api, RolesHandler checker)
        {
            _db = db;
            _vkApi = api;
            _checker = checker;
        }

        public async Task<string> Execute(Message msg)
        {
            if(msg.PeerId.Value == msg.FromId.Value)
            {
                return "Команда работает только в групповых чатах!";
            }

            var strBuilder = new StringBuilder();

            strBuilder.AppendLine("ТОП-10 участников чата по колличеству сообщений");
            strBuilder.AppendLine("_____________").AppendLine();

            var chat = _db.ChatRoles.Where(x => x.ChatVkID == msg.PeerId.Value).Select(x => x).OrderByDescending(x => x.AmountChatMessages).Take(10);

            int number = 0;
            foreach(var user in chat)
            {
                var vKUser = (await _vkApi.Users.GetAsync(new[] { (long)user.UserVkID})).FirstOrDefault();

                strBuilder.AppendLine($"{++number}. {vKUser.FirstName} {vKUser.LastName} - {user.AmountChatMessages}").AppendLine();

            }

            strBuilder.AppendLine("_____________").AppendLine();

            return strBuilder.ToString();
        }
    }
}