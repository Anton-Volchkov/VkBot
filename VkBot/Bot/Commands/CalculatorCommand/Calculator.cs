using Newtonsoft.Json;
using VkBot.Bot.Commands.CalculatorCommand;
using VkBot.Data.Abstractions;
using VkNet.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands;

public class Calculator : IBotCommand
{
    private readonly HttpClient _client;
    private readonly IVkApi _vkApi;

    public Calculator(IVkApi api)
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri("https://newton.now.sh/simplify/")
        };
        _vkApi = api;
    }

    public string[] Aliases { get; set; } = { "калькулятор", "посчитай" };

    public string Description { get; set; } =
        "Команда !Бот калькулятор вернёт вам результат выражения которое вы передадите." +
        "\nПример: !Бот калькулятор 2+2";

    public async Task<string> Execute(Message msg)
    {
        var user = (await _vkApi.Users.GetAsync(new[] { msg.FromId.Value })).FirstOrDefault();

        var split = msg.Text.Split(' ', 2); // [команда, параметры]

        if (split.Length < 2) return "Не все параметры указаны!";

        var expression = split[1].Trim();

        var query = await _client.GetAsync(expression);

        if (!query.IsSuccessStatusCode) return $"{user.FirstName} {user.LastName}, я не смог посчитать это... =(";

        var answer = JsonConvert.DeserializeObject<CalculatorAnswerModel>(await query.Content.ReadAsStringAsync())
            .Result;

        if (!int.TryParse(answer, out var result))
            return $"{user.FirstName} {user.LastName}, я не смог посчитать это... =(";

        return $"{user.FirstName} {user.LastName}, ответ вашего выражения = {result}";
    }
}