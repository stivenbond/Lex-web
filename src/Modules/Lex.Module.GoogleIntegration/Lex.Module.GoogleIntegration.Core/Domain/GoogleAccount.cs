using Lex.SharedKernel.Primitives;

namespace Lex.Module.GoogleIntegration.Core.Domain;

public sealed class GoogleAccount : AggregateRoot
{
    public string UserId { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string AccessToken { get; private set; } = string.Empty;
    public DateTime SyncedAt { get; private set; } = DateTime.UtcNow;

    private GoogleAccount() { }

    public static GoogleAccount Create(string userId, string email, string accessToken)
    {
        return new GoogleAccount
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Email = email,
            AccessToken = accessToken,
            SyncedAt = DateTime.UtcNow
        };
    }
}
