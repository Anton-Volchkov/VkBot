using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VkBot.Bot.Commands;
using VkBot.Data.Models;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace VkBot.botlogic
{
    public class Bot
    {
        public readonly MainContext _tm;
        private readonly IVkApi _vkApi;

        private readonly Random rnd = new Random();
        private static string text = "Расписание пусто!";
        private static bool hangrFire;

        public Bot(MainContext time, IVkApi vk)
        {
            _tm = time;
            _vkApi = vk;
        }

        public async Task HangleAsync()
        {
            hangrFire = true;
            await Task.Run(() => { Hangfire(); });
        }

        private void Hangfire()
        {
            while(true)
            {
                Thread.Sleep(1500000);
                _vkApi.Messages.Send(new MessagesSendParams
                {
                    RandomId = new DateTime().Millisecond + Guid.NewGuid().ToByteArray().Sum(x => x),
                    UserId = 494755463,
                    Message = "Я не сплю!"
                });
            }
        }

        public string SendMsgOrCommand(string message, Message prof)
        {
            if(!hangrFire)
            {
                HangleAsync();
            }

            var p = _vkApi.Users.Get(new[] { prof.FromId.Value }).FirstOrDefault();

            if(prof.FromId.Value == 285624980)
            {
                return "А тебе ничего не скажу.";
            }

            if(message.ToUpper().IndexOf("!БОТ КОМАНДЫ") >= 0)
            {
                return
                    "***КОМАНДЫ БОТА****<br>Перед каждой командой нужно ставить восклицательный знак.<br>Пример: !Команда <br>_____________<br><br>Бот звонок<br>Бот бибаметр<br>Бот бицепсметр<br>Бот рандом<br>Бот русская рулетка<br>Бот запомни расписание [В скобках указать расписание]<br>Бот расписание<br><br>_____________";
            }

            if(message.ToUpper().IndexOf("!БОТ БИБАМЕТР") >= 0)
            {
                return p.FirstName + " " + p.LastName + " имеет бибу " + rnd.Next(2, 32) + " см! NOT BAD!";
            }

            if(message.ToUpper().IndexOf("!БОТ БИЦЕПСМЕТР") >= 0)
            {
                return p.FirstName + " " + p.LastName + " имеет бицепс " + rnd.Next(15, 65) + " см в обхвате! NOT BAD!";
            }

            if(message.ToUpper().IndexOf("!БОТ РАНДОМ") >= 0)
            {
                return p.FirstName + " " + p.LastName + ", вам выпало рандомное число от 1 до 100 =  " +
                       rnd.Next(1, 100);
            }

            if(message.ToUpper().IndexOf("!БОТ РУССКАЯ РУЛЕТКА") >= 0)
            {
                if(rnd.Next(1, 7) == rnd.Next(1, 7))
                {
                    return p.FirstName + " " + p.LastName +
                           " был(а) убит(а) в русской рулетке! Press F to pay respects!";
                }

                return p.FirstName + " " + p.LastName + ", выжил в рулетке! Поздравим его!";
            }

            if(message.ToUpper().IndexOf("!БОТ ЗАПОМНИ РАСПИСАНИЕ ") == 0)
            {
                text = message.Substring(message.IndexOf("[") + 1, message.IndexOf(']') - message.IndexOf('[') - 1);

                return "Я запомнил расписание!";
            }

            if(message.ToUpper().IndexOf("!БОТ РАСПИСАНИЕ") == 0)
            {
                return text;
            }

            if(message.ToUpper().IndexOf("!БОТ ЗВОНОК") == 0)
            {
                var dt = DateTime.Now;
                return "Сейчас - " + dt.ToShortTimeString() +
                       "\n\n Расписание звонков \n 1) 8:00 - 8:45 / 8:55 - 9:40 \n 2) 9:50 - 10:35 / 10:45 - 11:30 \n 3)12:10 - 12:55 / 13:05 - 13:50 \n 4) 14:00 - 14:45 / 14:55 - 15:40 \n 5) 16:00 - 16:45 / 16:55 - 17:40 \n\n " +
                       dt.GetTime();
            }

            return "Я не знаю такой команды =( ";
        }
    }
}