using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lex.Module.AssessmentCreation.Core.Features.CreateSnapshot;

namespace Lex.Module.AssessmentCreation.Infrastructure.Controllers;

[ApiController]
[Route("api/assessments")]
[Authorize]
public sealed class AssessmentCreationController(IMediator mediator) : ControllerBase
{
    [HttpPost("snapshot")]
    public async Task<IActionResult> CreateSnapshot(CreateSnapshotCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        if (result.IsFailure) return BadRequest(result.Error);

        return Ok(new { SnapshotId = result.Value });
    }
}
