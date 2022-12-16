using Application.Commands.Abstractions;
using VkNet.Abstractions;
using VkNet.Model;

namespace Application.Commands;

public class Roulette : IBotCommand
{
    private readonly IVkApi _vkApi;

    public Roulette(IVkApi api)
    {
        _vkApi = api;
    }

    public string[] Aliases { get; set; } = { "рулетка" };

    public string Description { get; set; } =
        "Команда !Бот рулетка является развлекательно командой, она скажет вам удалось ли вам выжить в русской рулетке." +
        "\nПример: !Бот рулетка";

    public async Task<string> ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
    {
        var user = (await _vkApi.Users.GetAsync(new[] { msg.FromId.Value })).FirstOrDefault();

        var rnd = new System.Random();

        if (rnd.Next(1, 7) == rnd.Next(1, 7))
            return $"{user.FirstName} {user.LastName} погиб(ла) в рулетке...PRESS F TO PAY RESPECT!";
        else
            return $"{user.FirstName} {user.LastName} выжил(а) в рулетке! Поздравляем!";
    }
}