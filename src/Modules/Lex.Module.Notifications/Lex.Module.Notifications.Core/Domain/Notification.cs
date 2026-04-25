using Lex.SharedKernel.Primitives;

namespace Lex.Module.Notifications.Core.Domain;

public sealed class Notification : AggregateRoot
{
    public string UserId { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public string Message { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public bool IsRead { get; private set; }

    private Notification() { }

    public static Notification Create(string userId, string title, string message)
    {
        return new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = title,
            Message = message,
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        };
    }

    public void MarkAsRead()
    {
        IsRead = true;
    }
}
