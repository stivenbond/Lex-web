using MediatR;

namespace Lex.Module.Scheduling.Core.Features.SchedulingEvents;

public sealed record AcademicYearCreatedEvent(int YearId, string Name) : INotification;

public sealed record SlotAssignedEvent(int SlotId, int ClassId, string TeacherId, DateOnly Date, int PeriodId) : INotification;

public sealed record SlotRemovedEvent(int SlotId, int ClassId, string TeacherId) : INotification;

public interface ISchedulingEventPublisher
{
    Task PublishAcademicYearCreatedAsync(AcademicYearCreatedEvent @event, CancellationToken ct = default);
    Task PublishSlotAssignedAsync(SlotAssignedEvent @event, CancellationToken ct = default);
    Task PublishSlotRemovedAsync(SlotRemovedEvent @event, CancellationToken ct = default);
}
