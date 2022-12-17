using Application.Commands.Abstractions;
using VkNet.Abstractions;
using VkNet.Model;
using WikipediaApi;

namespace Application.Commands;

public class WikiPedia : IBotCommand
{
    private readonly IVkApi _vkApi;

    private readonly WikiApi _wikiApi;

    public WikiPedia(IVkApi vkApi, WikiApi wikiApi)
    {
        _vkApi = vkApi;
        _wikiApi = wikiApi;
    }

    public string[] Aliases { get; set; } = { "вики", "википедия" };

    public string Description { get; set; } =
        "Команда !Бот вики вернёт вам информацию по вашему с вопросы, если она там будет, с Википедии." +
        "\nПример: !Бот вики Компьютер ";

    public async Task<string> ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
    {
        var user = (await _vkApi.Users.GetAsync(new[] { msg.FromId.Value })).FirstOrDefault();

        var split = msg.Text.Split(' ', 2);

        if (split.Length < 2) return "Не все параметры указаны!";

        var titles = split[1];

        return $"{user.FirstName} {user.LastName}, {await _wikiApi.GetWikiAnswerAsync(titles)}";
    }
}