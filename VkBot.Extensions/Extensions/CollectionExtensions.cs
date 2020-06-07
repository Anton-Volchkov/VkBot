using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VkBot.Extensions
{
    public static class CollectionExtensions
    {
        public static List<T> TakeRandomElements<T>(this List<T> collection, int count)
        {
            var randomNumbers = new int[count];
            var outputArray = new List<T>();

            var rnd = new Random(new DateTime().Millisecond + Guid.NewGuid().ToByteArray().Sum(x => x));

            for(int i = 0; i < count; i++)
            {
                while(true)
                {
                    var number = rnd.Next(0, collection.Count);

                    if(!randomNumbers.Contains(number))
                    {
                        randomNumbers[i] = number;
                        break;
                    }
                }
            }

            foreach(var randomNumber in randomNumbers)
            {
                outputArray.Add(collection[randomNumber]);
            }

            return outputArray;
        }
    }
}
