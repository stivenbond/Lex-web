using FluentValidation;
using Lex.Module.Scheduling.Core.Domain;
using Lex.Module.Scheduling.Core.Features.SchedulingEvents;
using Lex.SharedKernel.Primitives;
using MediatR;

namespace Lex.Module.Scheduling.Core.Features.AssignClassToSlot;

public sealed record AssignClassToSlotCommand(
    int SlotId,
    int ClassId,
    string TeacherId,
    int ClassroomId,
    string Subject,
    string? TeacherName = null,
    string? Notes = null) : IRequest<Result<int>>;

internal sealed class AssignClassToSlotValidator : AbstractValidator<AssignClassToSlotCommand>
{
    public AssignClassToSlotValidator()
    {
        RuleFor(x => x.SlotId).GreaterThan(0);
        RuleFor(x => x.ClassId).GreaterThan(0);
        RuleFor(x => x.ClassroomId).GreaterThan(0);
        RuleFor(x => x.TeacherId).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Subject).NotEmpty().MaximumLength(100);
    }
}

internal sealed class AssignClassToSlotHandler(
    ISchedulingRepository repository,
    ISchedulingEventPublisher eventPublisher) : IRequestHandler<AssignClassToSlotCommand, Result<int>>
{
    public async Task<Result<int>> Handle(AssignClassToSlotCommand request, CancellationToken ct)
    {
        var slot = await repository.GetSlotByIdAsync(request.SlotId, ct);
        if (slot is null)
        {
            return Error.NotFound("Scheduling.Slot.NotFound", "Slot was not found.");
        }

        var hasConflict = await repository.HasConflictingPeriodAsync(
            request.SlotId,
            request.ClassId,
            request.ClassroomId,
            request.TeacherId,
            excludePeriodId: null,
            ct);

        if (hasConflict)
        {
            return Error.Conflict("Scheduling.Slot.Conflict", "Teacher, classroom, or class has a conflict in this slot.");
        }

        var period = Period.Create(
            request.SlotId,
            request.ClassId,
            request.ClassroomId,
            request.Subject,
            request.TeacherId,
            request.TeacherName,
            request.Notes);

        repository.AddPeriod(period);
        await repository.SaveChangesAsync(ct);

        var date = slot.Term.StartDate.AddDays(((int)slot.DayOfWeek - (int)slot.Term.StartDate.DayOfWeek + 7) % 7);
        await eventPublisher.PublishSlotAssignedAsync(
            new SlotAssignedEvent(slot.Id, request.ClassId, request.TeacherId, date, period.Id),
            ct);

        return period.Id;
    }
}
