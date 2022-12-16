using Microsoft.EntityFrameworkCore;
using VkBot.Domain;
using VkBot.Domain.Models;

namespace Services.Helpers;

public interface IRolesHelper
{
    Roles GetRoleByName(string name);
    string GetNameByRole(Roles roles);

    Task<bool> CheckAccessToCommandAsync(long? idUser, long? idChat, Roles needRole,
        CancellationToken cancellationToken = default);

    Task CheckUserInChatAsync(long? userId, long? chatId, CancellationToken cancellationToken = default);

    Task<Roles> GetUserRoleAsync(long? userId, long? chatId, CancellationToken cancellationToken = default);
}

public class RolesHelper : IRolesHelper
{
    private readonly MainContext _db;

    public RolesHelper(MainContext db)
    {
        _db = db;
    }

    public Roles GetRoleByName(string name)
    {
        if (name == "админ" || name == "администратор") return Roles.Admin;

        if (name == "модер" || name == "модератор") return Roles.Moderator;

        if (name == "редактор") return Roles.Editor;

        if (name == "юзер" || name == "пользователь") return Roles.User;

        if (name == "главный админ" || name == "гл. админ" || name == "гл админ") return Roles.GlAdmin;

        return Roles.RoleNotFound;
    }

    public string GetNameByRole(Roles roles)
    {
        if (roles == Roles.Admin) return "Администратор";

        if (roles == Roles.Moderator) return "Модератор";

        if (roles == Roles.GlAdmin) return "Главный Администратор";

        if (roles == Roles.Editor) return "Редактор";

        return "Пользователь";
    }

    public async Task<bool> CheckAccessToCommandAsync(long? idUser, long? idChat, Roles needRole,
        CancellationToken cancellationToken = default)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Vk == idUser, cancellationToken);
        if (user?.IsBotAdmin ?? false) return true;

        var role = (await _db.ChatRoles.FirstOrDefaultAsync(x => x.ChatVkID == idChat && x.UserVkID == idUser,
                cancellationToken))?
            .UserRole;

        return role >= needRole;
    }

    public async Task CheckUserInChatAsync(long? userId, long? chatId, CancellationToken cancellationToken = default)
    {
        var user = await _db.ChatRoles.FirstOrDefaultAsync(x => x.UserVkID == userId && x.ChatVkID == chatId,
            cancellationToken);

        if (user is null)
        {
            user = new ChatRoles
            {
                UserVkID = userId,
                ChatVkID = chatId,
                UserRole = Roles.User
            };

            await _db.ChatRoles.AddAsync(user, cancellationToken);
        }

        user.AmountChatMessages++;

        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<Roles> GetUserRoleAsync(long? userId, long? chatId, CancellationToken cancellationToken = default)
    {
        var roleUser =
            await _db.ChatRoles.FirstOrDefaultAsync(x => x.UserVkID == userId &&
                                                         x.ChatVkID == chatId, cancellationToken);
        return roleUser?.UserRole ?? Roles.User;
    }
}