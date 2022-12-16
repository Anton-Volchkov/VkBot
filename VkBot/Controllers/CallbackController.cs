﻿using Hangfire;
using Microsoft.AspNetCore.Mvc;
using VkBot.Bot;
using VkBot.Domain.Models;
using VkBot.PreProcessors.Abstractions;
using VkNet.Model;
using VkNet.Utils;

namespace VkBot.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CallbackController : ControllerBase
{
    private readonly ICommandHandler _commandHandler;
    private readonly IConfiguration _configuration;
    private readonly IEnumerable<ICommandPreprocessor> commandPreprocessors;

    public CallbackController(IConfiguration configuration, ICommandHandler commandHandler,
        IEnumerable<ICommandPreprocessor> commandPreprocessors)
    {
        _configuration = configuration;
        _commandHandler = commandHandler;
        this.commandPreprocessors = commandPreprocessors;
    }

    [HttpPost]
    public async Task<IActionResult> Callback([FromBody] Updates updates, CancellationToken cancellationToken)
    {
        if (updates.Secret != _configuration["secret"]) return Ok("Bad Secret Key");

        if (updates.Type == "confirmation") return Ok(_configuration["Config:Confirmation"]);

        if (updates.Type == "message_new")
        {
            var msg = Message.FromJson(new VkResponse(updates.Object));

            var canProceed = true;
            foreach (var commandPreprocessor in commandPreprocessors)
            {
                var result = await commandPreprocessor.ProcessAsync(msg, cancellationToken);

                if (!result) canProceed = false;
            }

            if (canProceed) BackgroundJob.Enqueue(() => _commandHandler.HandleAsync(msg, cancellationToken));
        }

        // Возвращаем "ok" серверу Callback API
        return Ok("ok");
    }
}