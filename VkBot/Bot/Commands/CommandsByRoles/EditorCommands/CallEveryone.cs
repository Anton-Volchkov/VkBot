using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkBot.Data.Models;
using VkNet.Abstractions;
using VkNet.Model;
using User = VkNet.Model.User;

namespace VkBot.Bot.Commands.CommandsByRoles.EditorCommands
{
    public class CallEveryone : IBotCommand
    {
        private readonly MainContext _db;
        private readonly IVkApi _vkApi;
        private readonly RolesHandler _checker;
        public string[] Aliases { get; set; } = { "созвать", "сбор" };

        public string Description { get; set; } =
            "Команда !Бот сбор созывает всех пользователей чата.\nПример: !Бот сбор или Бот созвать\n" +
            "ВАЖНО: КОМАНДА РАБОТАЕТ ТОЛЬКО С ПРАВАМИ РЕДАКТОРА И ВЫШЕ!";

        public CallEveryone(MainContext db, IVkApi api, RolesHandler checker)
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

            if(!await _checker.CheckAccessToCommand(msg.FromId.Value, msg.PeerId.Value, Roles.Editor))
            {
                return "Недосточно прав!";
            }

            var strBuilder = new StringBuilder();

            strBuilder.AppendLine("Общий сбор чата");
            strBuilder.AppendLine("_____________").AppendLine();

            ReadOnlyCollection<User> chat;
            try
            {
                 chat = _vkApi.Messages.GetConversationMembers(msg.PeerId.Value, new[] { "" })
                                 .Profiles;
            }
            catch(Exception e)
            {
                return
                    "Что-то пошло не так, возможно у меня не хвататет прав. Установите мне права администратора и попробуйте снова.";
            }
          

            foreach(var user in chat)
            {
                strBuilder.Append($"@id{user.Id} ({user.FirstName}) ");
            }
            
            strBuilder.AppendLine();

            strBuilder.AppendLine("_____________").AppendLine();

            return strBuilder.ToString();
        }
    }
}