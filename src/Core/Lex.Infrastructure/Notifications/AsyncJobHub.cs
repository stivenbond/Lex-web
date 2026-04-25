using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace Lex.Infrastructure.Notifications;

[Authorize]
public sealed class AsyncJobHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
        }
        await base.OnDisconnectedAsync(exception);
    }
}

public interface IAsyncJobClient
{
    Task JobStarted(Guid jobId, string jobName);
    Task JobProgressUpdated(Guid jobId, int progressPercentage, string? statusMessage);
    Task JobCompleted(Guid jobId);
    Task JobFailed(Guid jobId, string error);
}
