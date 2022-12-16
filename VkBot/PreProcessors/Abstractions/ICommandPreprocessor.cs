﻿using VkNet.Model;

namespace VkBot.PreProcessors.Abstractions;

public interface ICommandPreprocessor
{
    Task<bool> ProcessAsync(Message msg, CancellationToken cancellationToken = default);
}