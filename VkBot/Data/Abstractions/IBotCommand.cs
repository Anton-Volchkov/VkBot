using System.Threading.Tasks;
using VkNet.Model;

namespace VkBot.Data.Abstractions
{
    public interface IBotCommand
    {
        string[] Aliases { get; set; }

        string Description { get; set; }
        Task<string> Execute(Message msg); //TODO: string на класс
    }
}