using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VkBot.DataBase;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;
namespace VkBot.botlogic
{
    public class Bot
    {
        public readonly TimeTableContext _tm;
        public Bot(TimeTableContext time, IVkApi vk)
        {
            _tm = time;
            _vkApi = vk;
        }
        private readonly IVkApi _vkApi;
        Random rnd = new Random();
        static string text = "Расписание пусто!";
        static bool hanglefire = false;

     
        public async Task HangleAsync()
        {
            hanglefire = true;
            await Task.Run(()=> { Hangle(); });
        }
        private void Hangle()
        {
            while (true)
            {
                Thread.Sleep(1500000);
                _vkApi.Messages.Send(new MessagesSendParams
                {
                    RandomId = new DateTime().Millisecond + Guid.NewGuid().ToByteArray().Sum(x => x),
                    UserId = 494755463,
                    Message = "Я не сплю!",
                });

               
            }
           

        }

        public string SendMsgOrCommand(string message, Message prof)
        {
            if (!hanglefire)
            {
                HangleAsync();
            }
            var p = _vkApi.Users.Get(new long[] { Convert.ToInt64(prof.FromId.Value) }).FirstOrDefault();

            if (prof.FromId.Value == Convert.ToInt64(285624980))
            {
                return "А тебе ничего не скажу.";
            }
            if (message.ToUpper().IndexOf("!БОТ КОМАНДЫ") >= 0)
            {
                return "***КОМАНДЫ БОТА****<br>Перед каждой командой нужно ставить восклицательный знак.<br>Пример: !Команда <br>_____________<br><br>Бот звонок<br>Бот бибаметр<br>Бот бицепсметр<br>Бот рандом<br>Бот русская рулетка<br>Бот запомни расписание [В скобках указать расписание]<br>Бот расписание<br><br>_____________";
            }

            if (message.ToUpper().IndexOf("!БОТ БИБАМЕТР") >= 0)
            {
                   return p.FirstName +" "+p.LastName + " имеет бибу "+ rnd.Next(2,32) + " см! NOT BAD!";
            }

            if (message.ToUpper().IndexOf("!БОТ БИЦЕПСМЕТР") >= 0)
            {
                return p.FirstName + " " + p.LastName + " имеет бицепс " + rnd.Next(15,65) + " см в обхвате! NOT BAD!";
            }

            if (message.ToUpper().IndexOf("!БОТ РАНДОМ") >= 0)
            {
                return p.FirstName + " " + p.LastName + ", вам выпало рандомное число от 1 до 100 =  " + rnd.Next(1, 100) ;
            }

            if (message.ToUpper().IndexOf("!БОТ РУССКАЯ РУЛЕТКА") >= 0)
            {
                if (rnd.Next(1,7) == rnd.Next(1,7))
                {
                    return p.FirstName + " " + p.LastName + " был(а) убит(а) в русской рулетке! Press F to pay respects!";
                }

                return p.FirstName + " " + p.LastName + ", выжил в рулетке! Поздравим его!";
            }

            if (message.ToUpper().IndexOf("!БОТ ЗАПОМНИ РАСПИСАНИЕ ") == 0)
            {
                text = message.Substring(message.IndexOf("[") + 1, (message.IndexOf(']') - message.IndexOf('[') - 1));

              

                using (var db = new TimeTableContext())
                {

                }
                return "Я запомнил расписание!";
            }
            if (message.ToUpper().IndexOf("!БОТ РАСПИСАНИЕ") == 0)
            {

                return text;
            }

            if (message.ToUpper().IndexOf("!БОТ ЗВОНОК") == 0)
            {

                DateTime dt = DateTime.Now;

              
                return "Сейчас - "+ dt.ToShortTimeString() + "\n\n Расписание звонков \n 1) 8:00 - 8:45 / 8:55 - 9:40 \n 2) 9:50 - 10:35 / 10:45 - 11:30 \n 3)12:10 - 12:55 / 13:05 - 13:50 \n 4) 14:00 - 14:45 / 14:55 - 15:40 \n 5) 16:00 - 16:45 / 16:55 - 17:40 \n\n " + dt.GetTime() ;

            }

            return "Я не знаю такой команды =( ";

        }




            
        
    }

    
}
