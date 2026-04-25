using FluentValidation;
using Lex.Module.Scheduling.Core.Domain;
using Lex.SharedKernel.Primitives;
using MediatR;

namespace Lex.Module.Scheduling.Core.Features.UpdateSlot;

public sealed record UpdateSlotCommand(
    int SlotAssignmentId,
    int ClassId,
    string TeacherId,
    int ClassroomId,
    string Subject,
    string? TeacherName = null,
    string? Notes = null) : IRequest<Result<int>>;

internal sealed class UpdateSlotValidator : AbstractValidator<UpdateSlotCommand>
{
    public UpdateSlotValidator()
    {
        RuleFor(x => x.SlotAssignmentId).GreaterThan(0);
        RuleFor(x => x.ClassId).GreaterThan(0);
        RuleFor(x => x.ClassroomId).GreaterThan(0);
        RuleFor(x => x.TeacherId).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Subject).NotEmpty().MaximumLength(100);
    }
}

internal sealed class UpdateSlotHandler(ISchedulingRepository repository) : IRequestHandler<UpdateSlotCommand, Result<int>>
{
    public async Task<Result<int>> Handle(UpdateSlotCommand request, CancellationToken ct)
    {
        var period = await repository.GetPeriodByIdAsync(request.SlotAssignmentId, ct);
        if (period is null)
        {
            return Error.NotFound("Scheduling.Period.NotFound", "Slot assignment was not found.");
        }

        var hasConflict = await repository.HasConflictingPeriodAsync(
            period.SlotId,
            request.ClassId,
            request.ClassroomId,
            request.TeacherId,
            request.SlotAssignmentId,
            ct);

        if (hasConflict)
        {
            return Error.Conflict("Scheduling.Slot.Conflict", "Teacher, classroom, or class has a conflict in this slot.");
        }

        period.SectionId = request.ClassId;
        period.Update(request.Subject, request.TeacherId, request.TeacherName, request.ClassroomId, request.Notes);
        await repository.SaveChangesAsync(ct);
        return period.Id;
    }
}
