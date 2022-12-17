using Application.Commands.Abstractions;
using VkNet.Abstractions;
using VkNet.Model;

namespace Application.Commands;

public class BicepsMetr : IBotCommand
{
    private readonly IVkApi _vkApi;

    public BicepsMetr(IVkApi api)
    {
        _vkApi = api;
    }

    public string[] Aliases { get; set; } = { "бицепсметр", "битка", "бицметр" };

    public string Description { get; set; } =
        "Команда !Бот бицепсметр является развлекательной командой.Она скажет вам размер вашего бицепса по мнению бота." +
        "\nПример: !Бот бицепсметр";

    public async Task<string> ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
    {
        var user = (await _vkApi.Users.GetAsync(new[] { msg.FromId.Value })).FirstOrDefault();
        return
            $"{user.FirstName} {user.LastName} имеет бицепс {new System.Random().Next(10, 70)} см в обхвате! NOT BAD!";
    }
}