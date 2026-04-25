using Lex.Module.Scheduling.Core.Domain;
using Lex.Module.Scheduling.Core.Features.SchedulingEvents;
using Lex.SharedKernel.Primitives;
using MediatR;

namespace Lex.Module.Scheduling.Core.Features.RemoveSlot;

public sealed record RemoveSlotCommand(int SlotAssignmentId) : IRequest<Result>;

internal sealed class RemoveSlotHandler(
    ISchedulingRepository repository,
    ISchedulingEventPublisher eventPublisher) : IRequestHandler<RemoveSlotCommand, Result>
{
    public async Task<Result> Handle(RemoveSlotCommand request, CancellationToken ct)
    {
        var period = await repository.GetPeriodByIdAsync(request.SlotAssignmentId, ct);
        if (period is null)
        {
            return Error.NotFound("Scheduling.Period.NotFound", "Slot assignment was not found.");
        }

        repository.RemovePeriod(period);
        await repository.SaveChangesAsync(ct);

        await eventPublisher.PublishSlotRemovedAsync(
            new SlotRemovedEvent(period.SlotId, period.SectionId, period.TeacherId),
            ct);

        return Result.Success();
    }
}
