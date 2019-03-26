using System.Threading.Tasks;
using VkNet.Model;

namespace VkBot.Data.Abstractions
{
    public interface IBotCommand
    {
        // string Name { get; set; } // видимо, тебе это тут не нужно
        string[] Alliases { get; set; }

        Task<string> Execute(Message msg); //TODO: string на класс
    }
}