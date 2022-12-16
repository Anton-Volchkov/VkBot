using Application.Commands.Abstractions;
using Microsoft.EntityFrameworkCore;
using VkBot.Domain;
using VkNet.Abstractions;
using VkNet.Model;

namespace Application.Commands;

public class MailingWeather : IBotCommand
{
    private readonly MainContext _db;
    private readonly IVkApi _vkApi;

    public MailingWeather(MainContext db, IVkApi vkApi)
    {
        _db = db;
        _vkApi = vkApi;
    }

    public string[] Aliases { get; set; } = { "подписка", "отписка" };

    public string Description { get; set; } =
        "Команда !Бот подписка подписывает вас на рассылку погоды по указанному городу." +
        "\nПример: !Бот подписка Витебск ";

    public async Task<string> ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
    {
        var split = msg.Text.Split(' ', 2); // [команда, параметры]

        var command = split[0].Trim();
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Vk == msg.FromId.Value);
        var vkUser = (await _vkApi.Users.GetAsync(new[] { msg.FromId.Value })).FirstOrDefault();

        if (command == "отписка")
        {
            user.Weather = false;
            await _db.SaveChangesAsync();

            return $"{vkUser.FirstName} {vkUser.LastName}, вы отписались от рассылки погоды!";
        }

        if (split.Length < 2) return "Не все параметры указаны!";


        var city = split[1].Trim().ToLower();
        city = char.ToUpper(city[0]) + city.Substring(1);
        user.City = city;
        user.Weather = true;
        await _db.SaveChangesAsync();
        return
            $"{vkUser.FirstName} {vkUser.LastName}, подписка на рассылку погоды в городе {city} успешно оформлена!\n" +
            "ВАЖНО: Для корректной рассылки погоды у вас должен быть диалог с ботом. Если его нет, пожалуйста, напишите ему любое сообщение https://vk.com/kerlibot" +
            "\nЕсли Вы захотите отписаться от расслыки погоды, введите команду 'Бот отписка'.";
    }
}