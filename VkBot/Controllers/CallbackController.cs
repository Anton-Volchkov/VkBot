using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VkBot.Bot;
using VkBot.Data.Models;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;
using User = VkBot.Data.Models.User;

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

        private readonly MainContext _db;

        private readonly IVkApi _vkApi;

        private readonly CommandExecutor commandExec;

        //private Random rnd = new Random(); //TODO: почему нигде не используется

        public CallbackController(IVkApi vkApi, IConfiguration configuration, CommandExecutor cmdExec, MainContext db)
        {
            _vkApi = vkApi;
            _configuration = configuration;
            commandExec = cmdExec;
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Callback([FromBody] Updates updates)
        {
            if(updates.Secret != _configuration["SecretKey"])
            {
                return Ok("Bad Secret Key");
            }
            // Проверяем, что находится в поле "type" 
            if(updates.Type == "confirmation")

                // Отправляем строку для подтверждения 
            {
                return Ok(_configuration["Config:Confirmation"]);
            }

            if(updates.Type == "message_new")
            {
                var msg = Message.FromJson(new VkResponse(updates.Object));

                //если сообщение НЕ НАЧИНАЕТСЯ С ЭТОГО, то ничо не делаем
                if(!(msg.Text.ToLower().StartsWith("!бот") || msg.Text.ToLower().StartsWith("бот")))
                {
                    return Ok("ok");
                }

                //а если начинается, то вот
                msg.Text = string.Join(' ', msg.Text.Split(' ').Skip(1)); // убираем !бот

                if(_db.GetUsers().All(x => x.Vk != msg.FromId))
                {
                    await _db.Users.AddAsync(new User { Vk = msg.FromId });
                    await _db.SaveChangesAsync();
                }

                #region Проверка подписки
                //var subscription = _vkApi.Groups.IsMember("178921904", msg.FromId.Value, null, null).Select(x => x.Member).FirstOrDefault();
                //var text = subscription == false? "Подпишитесь на сообщество, чтобы пользоваться командами бота! \n \n https://vk.com/kerlibot" : await commandExec.HandleMessage(msg);
                #endregion

                var text = await commandExec.HandleMessage(msg);

                // Отправим в ответ полученный от пользователя текст
                _vkApi.Messages.Send(new MessagesSendParams
                {
                    //TODO: плохой рандом ид
                    RandomId = new DateTime().Millisecond + Guid.NewGuid().ToByteArray().Sum(x => x),
                    PeerId = msg.PeerId.Value,
                    Message = text
                });
            }

            // Возвращаем "ok" серверу Callback API
            return Ok("ok");
        }
    }
}