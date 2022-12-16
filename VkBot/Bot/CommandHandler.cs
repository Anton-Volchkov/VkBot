using VkBot.Domain;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;
using User = VkBot.Domain.Models.User;

namespace VkBot.Bot;

public interface ICommandHandler
{
    Task HandleAsync(Message msg, CancellationToken cancellationToken = default);
}

public class CommandHandler : ICommandHandler
{
    private readonly RolesHandler _checker;

    private readonly CommandExecutor _commandExecutor;
    private readonly MainContext _db;

    private readonly IVkApi _vkApi;

    public CommandHandler(MainContext db, IVkApi vkApi, CommandExecutor commandExecutor, RolesHandler checker)
    {
        _db = db;
        _vkApi = vkApi;
        _commandExecutor = commandExecutor;
        _checker = checker;
    }

    public async Task HandleAsync(Message msg, CancellationToken cancellationToken = default)
    {
        if (_db.GetUsers().All(x => x.Vk != msg.FromId))
        {
            await _db.Users.AddAsync(new User { Vk = msg.FromId }, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }

        if (msg.FromId.Value != msg.PeerId.Value) await _checker.CheckUserInChat(msg.FromId.Value, msg.PeerId.Value);

        //если сообщение НЕ НАЧИНАЕТСЯ С ЭТОГО, то ничо не делаем
        if (!(msg.Text.ToLower().StartsWith("!бот") || msg.Text.ToLower().StartsWith("бот"))) return;

        //а если начинается, то вот
        msg.Text = string.Join(' ', msg.Text.Split(' ').Skip(1)); // убираем !бот


        #region Проверка подписки

        //var subscription = _vkApi.Groups.IsMember("178921904", msg.FromId.Value, null, null).Select(x => x.Member).FirstOrDefault();
        //var text = subscription == false? "Подпишитесь на сообщество, чтобы пользоваться командами бота! \n \n https://vk.com/kerlibot" : await commandExec.HandleMessageAsync(msg);

        #endregion

        var text = await _commandExecutor.HandleMessageAsync(msg);

        // Отправим в ответ полученный от пользователя текст
        await _vkApi.Messages.SendAsync(new MessagesSendParams
        {
            //TODO: плохой рандом ид
            RandomId = new DateTime().Millisecond + Guid.NewGuid().ToByteArray().Sum(x => x),
            PeerId = msg.PeerId.Value,
            Message = text
        });
    }
}