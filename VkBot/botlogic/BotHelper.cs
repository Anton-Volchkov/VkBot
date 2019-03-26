using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VkBot.botlogic
{
    public static class BotHelper
    {


        public static string GetTime(this DateTime dt)
        {

            TimeSpan This = new TimeSpan(dt.Hour, dt.Minute, dt.Second);
            TimeSpan temp = new TimeSpan(0,0,0);
            string sendText = "";


            if (This > new TimeSpan(17, 40, 0))
            {
                sendText = "Учебный день закончился!";
                return sendText;
            }

            else if (This <= new TimeSpan(8, 45, 0))
            {
                TimeSpan ts = new TimeSpan(8, 45, 0);


                temp = (ts - This);

            }

            else if (This <= new TimeSpan(9, 40, 0))
            {
                TimeSpan ts = new TimeSpan(9, 40, 0);
                temp = (ts - This);
            }

            else if (This <= new TimeSpan(10, 35, 0))
            {
                TimeSpan ts = new TimeSpan(10, 35, 0);
                temp = (ts - This);
            }

            else if (This <= new TimeSpan(11, 30, 0))
            {
                TimeSpan ts = new TimeSpan(11, 30, 0);
                temp = (ts - This);
            }

            else if (This <= new TimeSpan(12, 55, 0))
            {
                TimeSpan ts = new TimeSpan(12, 55, 0);
                temp = (ts - This);
            }

            else if (This <= new TimeSpan(13, 50, 0))
            {
                TimeSpan ts = new TimeSpan(13, 50, 0);
                temp = (ts - This);
            }

            else if (This <= new TimeSpan(14, 45, 0))
            {
                TimeSpan ts = new TimeSpan(14, 45, 0);
                temp = (ts - This);
            }

            else if (This <= new TimeSpan(15, 40, 0))
            {
                TimeSpan ts = new TimeSpan(15, 40, 0);
                temp = (ts - This);
            }
            else if (This <= new TimeSpan(16,45, 0))
            {
                TimeSpan ts = new TimeSpan(16, 45, 0);
                temp = (ts - This);
            }
            else if (This <= new TimeSpan(17, 40, 0))
            {
                TimeSpan ts = new TimeSpan(17, 40, 0);
                temp = (ts - This);
            }





            if (temp > new TimeSpan(0,45,0))
            {
                sendText = "До начала пары примерно - " + (temp - new TimeSpan(0, 45, 0)).ToString();
            }
            else
            {
                sendText = "Занятие закончится примерно через - " + temp.ToString();
            }




            return sendText;

        }
    }
}
