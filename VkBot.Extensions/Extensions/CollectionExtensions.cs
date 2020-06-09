using System;
using System.Collections.Generic;
using System.Linq;

namespace VkBot.Extensions
{
    public static class CollectionExtensions
    {
        public static IEnumerable<T> TakeRandomElements<T>(this IEnumerable<T> collection, int count)
        {
            if(collection.Count() <= count)
            {
                return collection;
            }

            var randomNumbers = new int[count];
            var outputArray = new List<T>();

            var rnd = new Random(new DateTime().Millisecond + Guid.NewGuid().ToByteArray().Sum(x => x));

            for(var i = 0; i < count; i++)
                while(true)
                {
                    var number = rnd.Next(0, collection.Count());

                    if(!randomNumbers.Contains(number))
                    {
                        randomNumbers[i] = number;
                        outputArray.Add(collection.ElementAt(number));
                        break;
                    }
                }

            return outputArray;
        }
    }
}