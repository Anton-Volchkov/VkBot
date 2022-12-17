using VkNet.Model;

namespace Application.PreProcessors.Abstractions;

public interface ICommandPreprocessor
{
    Task<bool> ProcessAsync(Message msg, CancellationToken cancellationToken = default);
}