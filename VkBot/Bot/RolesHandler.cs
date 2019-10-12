using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VkBot.Bot.Help;
using VkBot.Data.Models;

namespace VkBot.Bot
{
    public class RolesHandler
    {
        private readonly MainContext _db;
        public RolesHandler(MainContext db)
        {
            _db = db;
        }

        public async Task<Roles> GetRolesByName(string name)
        {
            if(name == "админ" || name == "администратор")
            {
                return Roles.Admin;
            }
            else if(name == "модер" || name == "модератор")
            {
                return Roles.Moderator;
            }
            else if(name == "редактор")
            {
                return Roles.Editor;
            }
            else if(name == "юзер" || name == "пользователь")
            {
                return Roles.User;
            }
            else if(name == "главный админ" || name == "гл. админ" || name == "гл админ")
            {
                return Roles.GlAdmin;
            }
            else
            {
                return Roles.RoleNotFound;
            }
           
        }

        public async Task<string> GetNameByRoles(Roles roles)
        {
            if (roles == Roles.Admin)
            {
                return "Администратор";
            }
            else if (roles == Roles.Moderator)
            {
                return "Модератор";
            }
            else if (roles == Roles.GlAdmin)
            {
                return "Главный Администратор" ;
            }
            else if (roles == Roles.Editor)
            {
                return "Редактор";
            }
            else
            {
                return "Пользователь";
            }

        }

        public async Task<bool> CheckAccessToCommand(long? idUser, long? idChat, Roles needRole)
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
                user = new ChatRoles
                {
                    UserVkID = userId,
                    ChatVkID = chatId,
                    UserRole = Roles.User
                };

                await _db.ChatRoles.AddAsync(user);

            }

            user.AmountChatMessages++;

            await _db.SaveChangesAsync();
        }

        public async Task<Roles> GetUserRole(long? userID, long? chatID)
        {
            var roleUser =
                await _db.ChatRoles.FirstOrDefaultAsync(x => x.UserVkID == userID &&
                                                             x.ChatVkID == chatID);

            return roleUser.UserRole;
        }
    }
}
