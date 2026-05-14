using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lex.Module.ImportExport.Core.Features.StartImport;
using Lex.Module.ImportExport.Core.Features.CreateExport;

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

    [HttpPost("export")]
    public async Task<IActionResult> ExportData([FromBody] CreateExportCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        if (result.IsFailure) return BadRequest(result.Error);

        return Ok(new { ExportId = result.Value });
    }
}
