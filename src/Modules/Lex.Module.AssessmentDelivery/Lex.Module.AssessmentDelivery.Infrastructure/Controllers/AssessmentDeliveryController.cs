using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lex.Module.AssessmentDelivery.Core.Features.StartSession;

namespace Lex.Module.AssessmentDelivery.Infrastructure.Controllers;

[ApiController]
[Route("api/delivery")]
[Authorize]
public sealed class AssessmentDeliveryController(IMediator mediator) : ControllerBase
{
    [HttpPost("start")]
    public async Task<IActionResult> StartSession(StartSessionCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        if (result.IsFailure) return BadRequest(result.Error);

        return Ok(new { SessionId = result.Value });
    }
}
