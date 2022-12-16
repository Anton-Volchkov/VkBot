using VkNet.Model;

namespace Application.Commands.Abstractions;

public interface IBotCommand
{
    string[] Aliases { get; set; }
    string Description { get; set; }
    Task<string> ExecuteAsync(Message msg, CancellationToken cancellationToken = default);
}