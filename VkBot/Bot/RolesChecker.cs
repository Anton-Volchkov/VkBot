using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VkBot.Bot.Help;
using VkBot.Data.Models;

namespace VkBot.Bot
{
    public class RolesChecker
    {
        private readonly MainContext _db;
        public RolesChecker(MainContext db)
        {
            _db = db;
        }

        public async Task<bool> CheckAccessToRoles(long? idUser, long? idChat, Roles needRole)
        {
           
            if((await _db.Users.FirstOrDefaultAsync(x => x.Vk == idUser)).IsBotAdmin)
            {
                return true;
            }

            var role = (await _db.ChatRoles.FirstOrDefaultAsync(x => x.ChatVkID == idChat && x.UserVkID == idUser)).UserRole;

            return role >= needRole;
        }

        public async Task CheckUserInChat(long? userId, long? chatId)
        {
            var user = await _db.ChatRoles.FirstOrDefaultAsync(x => x.UserVkID == userId && x.ChatVkID == chatId);

            if(user is null)
            {
                await _db.ChatRoles.AddAsync(new ChatRoles
                {
                    UserVkID = userId,
                    ChatVkID = chatId,
                    UserRole = Roles.User
                });
                await _db.SaveChangesAsync();
            }
        }
    }
}
