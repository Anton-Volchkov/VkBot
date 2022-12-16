using Application.Commands.Abstractions;
using Newtonsoft.Json;
using VkNet.Abstractions;
using VkNet.Model;

namespace Application.Commands;

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

    public async Task<string> ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
    {
        var user = (await _vkApi.Users.GetAsync(new[] { msg.FromId.Value })).FirstOrDefault();

        var split = msg.Text.Split(' ', 2); // [команда, параметры]

        if (split.Length < 2) return "Не все параметры указаны!";

        var expression = split[1].Trim();

        var query = await _client.GetAsync(expression);

        if (!query.IsSuccessStatusCode) return $"{user.FirstName} {user.LastName}, я не смог посчитать это... =(";

        var answer = JsonConvert.DeserializeObject<CalculatorAnswerDTO>(await query.Content.ReadAsStringAsync())
            ?.Result;

        return !int.TryParse(answer, out var result)
            ? $"{user.FirstName} {user.LastName}, я не смог посчитать это... =("
            : $"{user.FirstName} {user.LastName}, ответ вашего выражения = {result}";
    }
}

public class CalculatorAnswerDTO
{
    [JsonProperty("operation")] public string Operation { get; set; }

    [JsonProperty("expression")] public string Expression { get; set; }

    [JsonProperty("result")] public string Result { get; set; }
}