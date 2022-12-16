using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Application.Commands.Abstractions;
using ImageFinder;
using ImageFinder.Models;
using Microsoft.Extensions.Logging;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace Application.Commands;

public class GetImage : IBotCommand
{
    private readonly ILogger<GetImage> _logger;
    private readonly ImageProvider _provider;

    private readonly IVkApi _vkApi;

    public GetImage(IVkApi vkApi, ImageProvider provider, ILogger<GetImage> logger)
    {
        _vkApi = vkApi;
        _provider = provider;
        _logger = logger;
    }

    public string[] Aliases { get; set; } = { "картинки", "картинка" };

    public string Description { get; set; } =
        "Команда !Бот картинки + *текст* покажет вам найденные картнки по вашему запросу" +
        "\nПример: !Бот картнки + кот";

    public async Task<string> ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
    {
        var split = msg.Text.Split(' ', 2); // [команда, параметры]

        if (split.Length < 2) return "Не все параметры указаны!";

        var urls = await _provider.GetImagesUrl(split[1], Browser.Yandex);

        if (urls.Count == 0)
        {
            _logger.LogCritical("Неудалось получить картинки через Yandex, пробуем получить через DuckDuckGo");
            urls = await _provider.GetImagesUrl(split[1], Browser.DuckDuckGo);

            if (urls.Count == 0) _logger.LogCritical("Неудалось получить картинки через DuckDuckGo");
        }
        else
        {
            _logger.LogInformation($"URL картинок получены, одна из них: {urls[0]}");
        }


        // Получить адрес сервера для загрузки картинок в сообщении
        try
        {
            var uploadServer = _vkApi.Photo.GetMessagesUploadServer(0);

            _logger.LogInformation("Адресс для загрузки получен");


            var imagePaths = await Task.WhenAll(urls.Select(x => UploadFile(uploadServer.UploadUrl,
                x, "jpg")));


            if (!imagePaths.Any()) return $"Картинок по запросу \"{split[1]}\" не найдено";

            // Сохранить загруженный файл

            var attachments =
                await Task.WhenAll(imagePaths.Select(x => Task.Run(() => _vkApi.Photo.SaveMessagesPhoto(x))));

            _vkApi.Messages.Send(new MessagesSendParams

            {
                PeerId = msg.PeerId.Value,

                Message = "",
                Attachments = attachments.SelectMany(x => x), //Вложение
                RandomId = new DateTime().Millisecond + Guid.NewGuid().ToByteArray().Sum(x => x)
            });
            return $"Картинки по запросу: \"{split[1]}\"";
        }
        catch (Exception e)
        {
            _logger.LogCritical($"Произошла ошибка: {e.Message}");
            return $"Упс... При поиске картинок по запросу: \"{split[1]}\" что-то пошло не так...";
        }
    }

    private async Task<string> UploadFile(string serverUrl, string file, string fileExtension)
    {
        // Получение массива байтов из файла
        var data = GetBytes(file);

        // Создание запроса на загрузку файла на сервер
        using var client = new HttpClient();
        var requestContent = new MultipartFormDataContent();
        var content = new ByteArrayContent(data);
        content.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
        requestContent.Add(content, "file", $"file.{fileExtension}");

        var response = client.PostAsync(serverUrl, requestContent).Result;
        return Encoding.Default.GetString(await response.Content.ReadAsByteArrayAsync());
    }

    private byte[] GetBytes(string fileUrl)
    {
        using var webClient = new WebClient();
        return webClient.DownloadData(fileUrl);
    }
}