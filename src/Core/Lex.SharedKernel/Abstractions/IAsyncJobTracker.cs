using Lex.SharedKernel.Primitives;

namespace Lex.SharedKernel.Abstractions;

public interface IAsyncJobTracker
{
    Task StartJobAsync(Guid jobId, string jobName, CancellationToken ct = default);
    Task UpdateProgressAsync(Guid jobId, int progressPercentage, string? statusMessage = null, CancellationToken ct = default);
    Task CompleteJobAsync(Guid jobId, CancellationToken ct = default);
    Task FailJobAsync(Guid jobId, string error, CancellationToken ct = default);
}
