using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lex.Module.Notifications.Core.Features.SendNotification;

namespace Lex.Module.Notifications.Infrastructure.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public sealed class NotificationsController(IMediator mediator) : ControllerBase
{
    [HttpPost("send")]
    public async Task<IActionResult> SendNotification([FromBody] SendNotificationCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        if (result.IsFailure) return BadRequest(result.Error);

        return Ok(new { NotificationId = result.Value });
    }
}
