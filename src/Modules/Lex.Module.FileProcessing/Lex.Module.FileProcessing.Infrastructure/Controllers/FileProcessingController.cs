using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lex.Module.FileProcessing.Core.Features.EnqueueProcessingJob;

namespace Lex.Module.FileProcessing.Infrastructure.Controllers;

[ApiController]
[Route("api/file-processing")]
[Authorize]
public sealed class FileProcessingController(IMediator mediator) : ControllerBase
{
    [HttpPost("enqueue")]
    public async Task<IActionResult> Enqueue(EnqueueProcessingCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        if (result.IsFailure) return BadRequest(result.Error);

        return Ok(new { JobId = result.Value });
    }
}
