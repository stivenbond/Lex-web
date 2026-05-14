using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lex.Module.Reporting.Core.Features.GenerateReport;

namespace Lex.Module.Reporting.Infrastructure.Controllers;

[ApiController]
[Route("api/reporting")]
[Authorize]
public sealed class ReportingController(IMediator mediator) : ControllerBase
{
    [HttpPost("generate")]
    public async Task<IActionResult> GenerateReport([FromBody] GenerateReportCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        if (result.IsFailure) return BadRequest(result.Error);

        return Ok(new { ReportId = result.Value });
    }
}
