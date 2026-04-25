using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lex.Module.GoogleIntegration.Core.Features.SyncGoogleDrive;

namespace Lex.Module.GoogleIntegration.Infrastructure.Controllers;

[ApiController]
[Route("api/google")]
[Authorize]
public sealed class GoogleIntegrationController(IMediator mediator) : ControllerBase
{
    [HttpPost("sync")]
    public async Task<IActionResult> Sync([FromBody] SyncRequest request, CancellationToken ct)
    {
        var command = new SyncGoogleDriveCommand(request.AccessToken);
        var result = await mediator.Send(command, ct);
        if (result.IsFailure) return BadRequest(result.Error);

        return Ok(new { SyncedCount = result.Value });
    }
}

public record SyncRequest(string AccessToken);
