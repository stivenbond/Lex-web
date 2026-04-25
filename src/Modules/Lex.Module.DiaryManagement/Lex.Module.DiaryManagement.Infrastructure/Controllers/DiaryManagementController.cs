using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lex.Module.DiaryManagement.Core.Features.CreateDiaryEntry;

namespace Lex.Module.DiaryManagement.Infrastructure.Controllers;

[ApiController]
[Route("api/diary")]
[Authorize]
public sealed class DiaryManagementController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateDiaryEntryCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        if (result.IsFailure) return BadRequest(result.Error);

        return Ok(new { EntryId = result.Value });
    }
}
