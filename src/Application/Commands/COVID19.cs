using Application.Commands.Abstractions;
using CoronaVirus;
using VkNet.Model;

namespace Application.Commands;

public class COVID19 : IBotCommand
{
    private readonly CoronaInfo _coronaInfo;

    public COVID19(CoronaInfo coronaInfo)
    {
        _coronaInfo = coronaInfo;
    }

    public string[] Aliases { get; set; } = { "вирус", "коронавирус" };

    public string Description { get; set; } =
        "Команда !Бот вирус расскажет вам общую ситуацию или ситуацию в конкретной стране связанной с COVID-19" +
        "\nПример: !Бот вирус ИЛИ Бот вирус + Страна";

    public async Task<string> ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
    {
        var split = msg.Text.Split(' ', 2); // [команда, параметры]

        if (split.Length < 2) return "Не все параметры указаны!";

        return await _coronaInfo.GetCoronaVirusInfoAsync(split[1], cancellationToken);
    }
}