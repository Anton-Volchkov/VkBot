using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkBot.Data.Models;
using VkNet.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class CheckOnline : IBotCommand
    {
        private readonly MainContext _db;
        private readonly IVkApi _vkApi;
        public string[] Alliases { get; set; } = { "онлайн" };
        public string Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public CheckOnline(MainContext db, IVkApi api)
        {
            _db = db;
            _vkApi = api;
        }
        public async Task<string> Execute(Message msg)
        {
            var users = _db.ChatRoles.Where(x => x.ChatVkID == msg.PeerId).ToArray();
            StringBuilder userOnline = new StringBuilder();


            userOnline.AppendLine("Пользователи чата онлайн");
            userOnline.AppendLine("_____________").AppendLine();

            foreach(var user in users)
            {
                if ((bool)_vkApi.Messages.GetLastActivity((long)user.UserVkID).IsOnline)
                {
                    var VkUser = (await _vkApi.Users.GetAsync(new[] { (long)user.UserVkID })).FirstOrDefault();

                    userOnline.AppendLine($"{VkUser.FirstName} {VkUser.LastName}");
                }

                await Task.Delay(60);
            }

            userOnline.AppendLine("_____________").AppendLine();

            return userOnline.ToString();
        }
    }
}
