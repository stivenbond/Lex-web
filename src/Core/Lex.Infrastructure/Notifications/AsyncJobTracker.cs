using Microsoft.AspNetCore.SignalR;
using Lex.SharedKernel.Abstractions;
using Lex.Infrastructure.Notifications;

namespace Lex.Infrastructure.Notifications;

internal sealed class AsyncJobTracker(
    IHubContext<AsyncJobHub> hubContext,
    ICurrentUser currentUser) : IAsyncJobTracker
{
    public async Task StartJobAsync(Guid jobId, string jobName, CancellationToken ct = default)
    {
        await NotifyUser(j => hubContext.Clients.Group(j.Group).SendAsync("JobStarted", jobId, jobName, ct));
    }

    public async Task UpdateProgressAsync(Guid jobId, int progressPercentage, string? statusMessage = null, CancellationToken ct = default)
    {
        await NotifyUser(j => hubContext.Clients.Group(j.Group).SendAsync("JobProgressUpdated", jobId, progressPercentage, statusMessage, ct));
    }

    public async Task CompleteJobAsync(Guid jobId, CancellationToken ct = default)
    {
        await NotifyUser(j => hubContext.Clients.Group(j.Group).SendAsync("JobCompleted", jobId, ct));
    }

    public async Task FailJobAsync(Guid jobId, string error, CancellationToken ct = default)
    {
        await NotifyUser(j => hubContext.Clients.Group(j.Group).SendAsync("JobFailed", jobId, error, ct));
    }

    private async Task NotifyUser(Func<(string Group, Guid UserId), Task> notify)
    {
        var userId = currentUser.Id;
        if (userId != Guid.Empty)
        {
            await notify(($"user_{userId}", userId));
        }
    }
}
