using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkBot.Data.Models;
using VkNet.Abstractions;
using VkNet.Enums.Filters;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class CheckOnlineUser : IBotCommand
    {
        private readonly MainContext _db;
        private readonly IVkApi _vkApi;
        public string[] Alliases { get; set; } = { "онлайн" };

        public string Description { get; set; } =
            "Команда !Бот онлайн скажет вам кто онлайн в беседе.\nПример: !Бот онлайн";

        public CheckOnlineUser(MainContext db, IVkApi api)
        {
            _db = db;
            _vkApi = api;
        }

        public async Task<string> Execute(Message msg)
        {
            if (msg.PeerId.Value == msg.FromId.Value)
            {
                return "Команда работает только в групповых чатах!";
            }

            var users = _db.ChatRoles.Where(x => x.ChatVkID == msg.PeerId).ToArray();
            var userOnline = new StringBuilder();


            userOnline.AppendLine("Пользователи чата онлайн");
            userOnline.AppendLine("_____________").AppendLine();

            foreach(var user in users)
            {
                var VkUser = (await _vkApi.Users.GetAsync(new[] { (long) user.UserVkID }, ProfileFields.Online)).FirstOrDefault();

                if(!VkUser.Online.HasValue)
                {
                    continue;
                }

                if(VkUser.Online.Value)
                {
                    if(VkUser.OnlineMobile.HasValue)
                    {
                        userOnline.AppendLine($"{VkUser.FirstName} {VkUser.LastName} (📱)");
                    }
                    else
                    {
                        userOnline.AppendLine($"{VkUser.FirstName} {VkUser.LastName} (💻)");
                    }
                }

            }

            userOnline.AppendLine("_____________").AppendLine();

            return userOnline.ToString();
        }
    }
}