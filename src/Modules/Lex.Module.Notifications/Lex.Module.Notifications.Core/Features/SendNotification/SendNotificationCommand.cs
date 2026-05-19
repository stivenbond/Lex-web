using Lex.Module.Notifications.Core.Domain;
using Lex.SharedKernel.Primitives;
using MediatR;

namespace Lex.Module.Notifications.Core.Features.SendNotification;

public sealed record SendNotificationCommand(string UserId, string Title, string Message) : IRequest<Result<Guid>>;

internal sealed class SendNotificationCommandHandler(INotificationRepository repository) : IRequestHandler<SendNotificationCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = Notification.Create(request.UserId, request.Title, request.Message);
        repository.AddNotification(notification);
        await repository.SaveChangesAsync(cancellationToken);
        return notification.Id;
    }
}
