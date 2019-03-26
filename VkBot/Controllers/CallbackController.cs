using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VkBot.botlogic;
using VkBot.Data.Models;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace VkBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallbackController : ControllerBase
    {
        /// <summary>
        ///     Конфигурация приложения
        /// </summary>
        private readonly IConfiguration _configuration;

        private Random rnd = new Random();

        private readonly IVkApi _vkApi;
        private readonly Bot bot;

        public CallbackController(IVkApi vkApi, IConfiguration configuration)
        {
            _vkApi = vkApi;
            _configuration = configuration;
            bot = new Bot(vkApi);
        }

        [HttpPost]
        public IActionResult Callback([FromBody] Updates updates)
        {
            // Проверяем, что находится в поле "type" 
            switch(updates.Type)
            {
                // Если это уведомление для подтверждения адреса
                case "confirmation":
                {
                    // Отправляем строку для подтверждения 
                    return Ok(_configuration["Config:Confirmation"]);
                }
                case "message_new":
                {
                    // Десериализация
                    var msg = Message.FromJson(new VkResponse(updates.Object));

                    if(msg.Text.ToUpper().IndexOf("!БОТ") >= 0)
                    {
                        var text = bot.SendMsgOrCommand(msg.Text, msg);

                        // Отправим в ответ полученный от пользователя текст
                        _vkApi.Messages.Send(new MessagesSendParams
                        {
                            //TODO: плохой рандом ид
                            RandomId = new DateTime().Millisecond + Guid.NewGuid().ToByteArray().Sum(x => x),
                            PeerId = msg.PeerId.Value,
                            Message = text
                        });
                    }

                    break;
                }
            }

            // Возвращаем "ok" серверу Callback API
            return Ok("ok");
        }
    }
}