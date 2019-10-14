using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public Roles GetRoleByName(string name)
        {
            if(name == "админ" || name == "администратор")
            {
                return Roles.Admin;
            }

            if(name == "модер" || name == "модератор")
            {
                return Roles.Moderator;
            }

            if(name == "редактор")
            {
                return Roles.Editor;
            }

            if(name == "юзер" || name == "пользователь")
            {
                return Roles.User;
            }

            if(name == "главный админ" || name == "гл. админ" || name == "гл админ")
            {
                return Roles.GlAdmin;
            }

            return Roles.RoleNotFound;
        }

        public string GetNameByRole(Roles roles)
        {
            if(roles == Roles.Admin)
            {
                return "Администратор";
            }

            if(roles == Roles.Moderator)
            {
                return "Модератор";
            }

            if(roles == Roles.GlAdmin)
            {
                return "Главный Администратор";
            }

            if(roles == Roles.Editor)
            {
                return "Редактор";
            }

            return "Пользователь";
        }

        public async Task<bool> CheckAccessToCommand(long? idUser, long? idChat, Roles needRole)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Vk == idUser);
            if(user.IsBotAdmin)
            {
                return true;
            }

            var role = (await _db.ChatRoles.FirstOrDefaultAsync(x => x.ChatVkID == idChat && x.UserVkID == idUser))
                    .UserRole;

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

        public async Task<Roles> GetUserRole(long? userId, long? chatId)
        {
            var roleUser =
                    await _db.ChatRoles.FirstOrDefaultAsync(x => x.UserVkID == userId &&
                                                                 x.ChatVkID == chatId);
            if(roleUser is null)
            {
                return Roles.User;
            }
            return roleUser.UserRole;
        }
    }
}