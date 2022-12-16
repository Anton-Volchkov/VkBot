using Microsoft.EntityFrameworkCore;
using VkBot.Data.Abstractions;
using VkBot.Domain;
using VkBot.Domain.Models;
using VkNet.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands;

public class GetRights : IBotCommand
{
    private readonly MainContext _db;
    private readonly IVkApi _vkApi;

    public GetRights(MainContext db, IVkApi api)
    {
        _db = db;
        _vkApi = api;
    }

    public string[] Aliases { get; set; } = { "получить" };

    public string Description { get; set; } =
        "Команда !Бот получить права выдает в боте права главного администратора создателю беседы" +
        "\nПример: !Бот получить права ";

    public async Task<string> Execute(Message msg)
    {
        if (msg.PeerId.Value == msg.FromId.Value) return "Команда работает только в групповых чатах!";

        var split = msg.Text.Split(' ', 2); // [команда, параметры]

        if (split[1] != "права") return "Я не знаю такой команды =(";

        long[] chat;
        try
        {
            chat = _vkApi.Messages.GetConversationMembers(msg.PeerId.Value, new List<string> { "is_admin" }).Items
                .Where(x => x.IsAdmin).Select(x => x.MemberId).ToArray();
        }
        catch (Exception)
        {
            return
                "Что-то пошло не так, возможно у меня не хвататет прав. Установите мне права администратора и попробуйте снова.";
        }

        if (!chat.Contains(msg.FromId.Value)) return "Этой командой может воспользоваться только администратор!";

        foreach (var item in chat)
        {
            var admin = await _db.ChatRoles.FirstOrDefaultAsync(x => x.UserVkID == msg.FromId.Value &&
                                                                     x.ChatVkID == msg.PeerId.Value);
            admin.UserRole = Roles.GlAdmin;
        }

        await _db.SaveChangesAsync();

        return "Права к боту выданы!";
    }
}