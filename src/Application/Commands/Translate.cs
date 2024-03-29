﻿using Application.Commands.Abstractions;
using VkNet.Abstractions;
using VkNet.Model;
using YandexTranslator;

namespace Application.Commands;

public class Translate : IBotCommand
{
    private readonly Translator _translator;

    private readonly IVkApi _vkApi;

    public Translate(IVkApi vkApi, Translator translator)
    {
        _vkApi = vkApi;
        _translator = translator;
    }

    public string[] Aliases { get; set; } = { "перевод", "переводчик" };

    public string Description { get; set; } =
        "Команда !Бот перевод переведёт ваш текст с выбранного языка на выбранный язык." +
        "\nПример: !Бот перевод ru-en Привет\nP.S Переведёт с Русского на Английский текст Привет";

    public async Task<string> ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
    {
        var split = msg.Text.Split(' ', 3); // [команда, параметры, текст]

        if (split.Length < 3) return "Не все параметры указаны!";

        var user = (await _vkApi.Users.GetAsync(new[] { msg.FromId.Value })).FirstOrDefault();

        if (split.Length != 3) return $"{user.FirstName} {user.LastName}, проверьте введённые данные.";

        var lang = split[1].Trim().ToLower();

        var text = split[2].Trim();

        return
            $"{user.FirstName} {user.LastName}, перевод вашего текста\n\n{await _translator.Translate(text, lang)}";
    }
}