namespace Lex.Module.Notifications.Core.Domain;

public interface INotificationRepository
{
    void AddNotification(Notification notification);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
