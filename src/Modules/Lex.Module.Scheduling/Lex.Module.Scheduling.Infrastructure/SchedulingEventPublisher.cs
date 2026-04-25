using Lex.Module.Scheduling.Core.Features.SchedulingEvents;
using MassTransit;

namespace Lex.Module.Scheduling;

internal sealed class SchedulingEventPublisher(IPublishEndpoint publishEndpoint) : ISchedulingEventPublisher
{
    public Task PublishAcademicYearCreatedAsync(AcademicYearCreatedEvent @event, CancellationToken ct = default) =>
        publishEndpoint.Publish(@event, ct);

    public Task PublishSlotAssignedAsync(SlotAssignedEvent @event, CancellationToken ct = default) =>
        publishEndpoint.Publish(@event, ct);

    public Task PublishSlotRemovedAsync(SlotRemovedEvent @event, CancellationToken ct = default) =>
        publishEndpoint.Publish(@event, ct);
}
