using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ImageFinder;
using VkBot.Data.Abstractions;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace VkBot.Bot.Commands
{
    public class GetImage : IBotCommand
    {
        public string[] Aliases { get; set; } = { "картинки", "картинка" };

        public string Description { get; set; } =
            "Команда !Бот картинки + *текст* покажет вам найденные картнки по вашему запросу" +
            "\nПример: !Бот картнки + кот";

        private readonly IVkApi _vkApi;
        private readonly ImageProvider _provider;

        public GetImage(IVkApi vkApi, ImageProvider provider)
        {
            _vkApi = vkApi;
            _provider = provider;
        }

        public async Task<string> Execute(Message msg)
        {
            var split = msg.Text.Split(' ', 2); // [команда, параметры]

            var url = _provider.GetImagesUrl(split[1]);

            // Получить адрес сервера для загрузки картинок в сообщении
            var uploadServer = _vkApi.Photo.GetMessagesUploadServer(msg.FromId.Value);

            // Загрузить картинки на сервер VK.
            var imagePath = new List<string>();

            foreach(var item in url)
                imagePath.Add(await UploadFile(uploadServer.UploadUrl,
                                               item, "jpg"));

            var attachment = new List<IReadOnlyCollection<Photo>>();

            if(imagePath.Count > 0)
            {
                // Сохранить загруженный файл
                foreach(var path in imagePath) attachment.Add(_vkApi.Photo.SaveMessagesPhoto(path));


               
                _vkApi.Messages.Send(new MessagesSendParams
                {
                    UserId = msg.PeerId.Value, 
                    Message = "", 
                    Attachments = attachment.SelectMany(x => x), //Вложение
                    RandomId = new DateTime().Millisecond + Guid.NewGuid().ToByteArray().Sum(x => x)
                });
                return $"Картинки по запросу: \"{split[1]}\"";
            }

            return $"Картинок по запросу \"{split[1]}\" не найдено";
        }

        private async Task<string> UploadFile(string serverUrl, string file, string fileExtension)
        {
            // Получение массива байтов из файла
            var data = GetBytes(file);

            // Создание запроса на загрузку файла на сервер
            using(var client = new HttpClient())
            {
                var requestContent = new MultipartFormDataContent();
                var content = new ByteArrayContent(data);
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                requestContent.Add(content, "file", $"file.{fileExtension}");

                var response = client.PostAsync(serverUrl, requestContent).Result;
                return Encoding.Default.GetString(await response.Content.ReadAsByteArrayAsync());
            }
        }

        private byte[] GetBytes(string fileUrl)
        {
            using(var webClient = new WebClient())
            {
                return webClient.DownloadData(fileUrl);
            }
        }
    }
}