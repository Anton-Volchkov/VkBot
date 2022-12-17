using Application;
using Application.PreProcessors.Abstractions;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using VkBot.Domain.Models;
using VkNet.Model;
using VkNet.Utils;

namespace VkBot.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CallbackController : ControllerBase
{
    private readonly ICommandHandler _commandHandler;
    private readonly IEnumerable<ICommandPreprocessor> _commandPreprocessors;
    private readonly IConfiguration _configuration;

    public CallbackController(IConfiguration configuration, ICommandHandler commandHandler,
        IEnumerable<ICommandPreprocessor> commandPreprocessors)
    {
        _configuration = configuration;
        _commandHandler = commandHandler;
        _commandPreprocessors = commandPreprocessors;
    }

    [HttpPost]
    public async Task<IActionResult> Callback([FromBody] Updates updates, CancellationToken cancellationToken)
    {
        if (updates.Secret != _configuration["secret"]) return Ok("Bad Secret Key");

        if (updates.Type == "confirmation") return Ok(_configuration["Config:Confirmation"]);

        if (updates.Type == "message_new")
        {
            var msg = Message.FromJson(new VkResponse(updates.Object));

            foreach (var commandPreprocessor in _commandPreprocessors)
            {
                var canProcess = await commandPreprocessor.ProcessAsync(msg, cancellationToken);

                if (!canProcess) return Ok("ok");
            }

            BackgroundJob.Enqueue(() => _commandHandler.HandleAsync(msg, cancellationToken));
        }

        // Возвращаем "ok" серверу Callback API
        return Ok("ok");
    }
}