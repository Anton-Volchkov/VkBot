using Application.Commands.Abstractions;
using VkNet.Abstractions;
using VkNet.Model;

namespace Application.Commands;

public class Random : IBotCommand
{
    private readonly IVkApi _vkApi;

    public Random(IVkApi api)
    {
        _vkApi = api;
    }

    public string[] Aliases { get; set; } = { "рандом" };

    public string Description { get; set; } =
        "Команда !Бот рандом возвращает вам случайно число в диапазоне от 1 до 100." +
        "\nПример: !Бот рандом ";

    public async Task<string> ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
    {
        var user = (await _vkApi.Users.GetAsync(new[] { msg.FromId.Value })).FirstOrDefault();

        return
            $"{user.FirstName} {user.LastName}, в промежутке от 1 до 100 выпало число - {new System.Random().Next(1, 100)}";
    }
}