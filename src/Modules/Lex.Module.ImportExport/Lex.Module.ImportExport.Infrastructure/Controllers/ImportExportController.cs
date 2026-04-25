using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lex.Module.ImportExport.Core.Features.StartImport;

namespace Lex.Module.ImportExport.Infrastructure.Controllers;

[ApiController]
[Route("api/import-export")]
[Authorize]
public sealed class ImportExportController(IMediator mediator) : ControllerBase
{
    [HttpPost("import")]
    public async Task<IActionResult> Import(ImportAssessmentCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        if (result.IsFailure) return BadRequest(result.Error);

        return Ok(new { ImportJobId = result.Value });
    }
}
