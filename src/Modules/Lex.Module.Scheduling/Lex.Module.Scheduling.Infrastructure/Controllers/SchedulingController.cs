using Lex.Module.Scheduling.Core.Features.AssignClassToSlot;
using Lex.Module.Scheduling.Core.Features.CreateAcademicYear;
using Lex.Module.Scheduling.Core.Features.CreatePeriod;
using Lex.Module.Scheduling.Core.Features.CreateTerm;
using Lex.Module.Scheduling.Core.Features.QuerySchedule;
using Lex.Module.Scheduling.Core.Features.RemoveSlot;
using Lex.Module.Scheduling.Core.Features.UpdateSlot;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lex.Module.Scheduling.Infrastructure.Controllers;

[ApiController]
[Route("api/scheduling")]
[Authorize]
public sealed class SchedulingController(IMediator mediator) : ControllerBase
{
    [HttpPost("academic-years")]
    [Authorize(Policy = SchedulingPermissions.ManageCalendar)]
    public async Task<IActionResult> CreateAcademicYear([FromBody] CreateAcademicYearCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        if (result.IsFailure) return BadRequest(result.Error);

        return Ok(new { AcademicYearId = result.Value });
    }

    [HttpPost("terms")]
    [Authorize(Policy = SchedulingPermissions.ManageCalendar)]
    public async Task<IActionResult> CreateTerm([FromBody] CreateTermCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        if (result.IsFailure) return BadRequest(result.Error);

        return Ok(new { TermId = result.Value });
    }

    [HttpPost("periods")]
    [Authorize(Policy = SchedulingPermissions.ManageCalendar)]
    public async Task<IActionResult> CreatePeriod([FromBody] CreatePeriodCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        if (result.IsFailure) return BadRequest(result.Error);

        return Ok(new { PeriodId = result.Value });
    }

    [HttpPost("slots/assign")]
    [Authorize(Policy = SchedulingPermissions.AssignSlots)]
    public async Task<IActionResult> AssignClassToSlot([FromBody] AssignClassToSlotCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        if (result.IsFailure) return BadRequest(result.Error);

        return Ok(new { AssignmentId = result.Value });
    }

    [HttpPatch("slots/{id:int}")]
    [Authorize(Policy = SchedulingPermissions.AssignSlots)]
    public async Task<IActionResult> UpdateSlot(int id, [FromBody] UpdateSlotCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command with { SlotAssignmentId = id }, ct);
        if (result.IsFailure) return BadRequest(result.Error);

        return Ok(new { AssignmentId = result.Value });
    }

    [HttpDelete("slots/{id:int}")]
    [Authorize(Policy = SchedulingPermissions.AssignSlots)]
    public async Task<IActionResult> RemoveSlot(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new RemoveSlotCommand(id), ct);
        if (result.IsFailure) return BadRequest(result.Error);

        return NoContent();
    }

    [HttpGet("schedule/class/{classId:int}")]
    [Authorize(Policy = SchedulingPermissions.ViewSchedule)]
    public async Task<IActionResult> GetClassSchedule(int classId, [FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate, CancellationToken ct)
    {
        var result = await mediator.Send(new GetScheduleForClassQuery(classId, startDate, endDate), ct);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpGet("schedule/teacher/{teacherId}")]
    [Authorize(Policy = SchedulingPermissions.ViewSchedule)]
    public async Task<IActionResult> GetTeacherSchedule(string teacherId, [FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate, CancellationToken ct)
    {
        var result = await mediator.Send(new GetScheduleForTeacherQuery(teacherId, startDate, endDate), ct);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpGet("periods/date/{date}")]
    [Authorize(Policy = SchedulingPermissions.ViewSchedule)]
    public async Task<IActionResult> GetPeriodsForDate(DateOnly date, CancellationToken ct)
    {
        var result = await mediator.Send(new GetPeriodsForDateQuery(date), ct);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result.Value);
    }
}
