using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VkBot.Data.Abstractions;
using VkBot.Data.Models;
using VkNet.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class Statistics : IBotCommand
    {
        private readonly MainContext _db;
        private readonly IVkApi _vkApi;
        private readonly RolesHandler _checker;

        public string[] Aliases { get; set; } = { "стат", "статистика", "стата" };

        public string Description { get; set; } =
            "Команда !Бот стат + пересланное сообщение скажет вам о статистике пользователя в этом чате, чьё сообщение вы переслали" +
            "\nПример: !Бот стат + пересланное сообщение";

        public Statistics(MainContext db, IVkApi api, RolesHandler checker)
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


            var forwardMessage = msg.ForwardedMessages.Count == 0 ? msg.ReplyMessage : msg.ForwardedMessages[0];

            if(forwardMessage is null)
            {
                forwardMessage = msg; //Если пересланного сообщения нет, то работаем с тем, кто написал его.
            }

            var user =
                await _db.ChatRoles.FirstOrDefaultAsync(x => x.UserVkID == forwardMessage.FromId.Value &&
                                                             x.ChatVkID == msg.PeerId.Value);

            if(user is null)
            {
                return "Данный пользователь ещё ничего не написал в этом чате!";
            }

            var VkUser = (await _vkApi.Users.GetAsync(new[] { forwardMessage.FromId.Value })).FirstOrDefault();

            var userGroup = (await _db.Users.FirstOrDefaultAsync(x => x.Vk == forwardMessage.FromId.Value))?.Group;

            userGroup = string.IsNullOrWhiteSpace(userGroup) ? "Группа не установлена." : userGroup.ToUpper();

            var sb = new StringBuilder();

            var status = string.IsNullOrWhiteSpace(user.Status) ? "Не установлен" : user.Status;

            sb.AppendLine($"Статистика для пользователя - {VkUser.FirstName} {VkUser.LastName}");
            sb.AppendLine("_______________").AppendLine();
            sb.AppendLine($"Роль в чате: {_checker.GetNameByRole(user.UserRole)}").AppendLine();
            sb.AppendLine($"Отправлено сообщений в этом чате: {user.AmountChatMessages}").AppendLine();
            sb.AppendLine($"Статус: {status}").AppendLine();
            sb.AppendLine($"Группа: {userGroup}").AppendLine();

            sb.AppendLine("_______________");

            return sb.ToString();
        }
    }
}