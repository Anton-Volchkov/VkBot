using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkNet.Model;

namespace VkBot.Data.Abstractions
{
   public interface IInfo : IBotCommand
   {
        //Вспомогательный класс для INFO для избежания рекурсии
   }
}
