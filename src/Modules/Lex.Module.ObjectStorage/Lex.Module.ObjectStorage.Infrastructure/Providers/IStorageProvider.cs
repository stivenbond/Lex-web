using Lex.SharedKernel.Primitives;

namespace Lex.Module.ObjectStorage.Infrastructure.Providers;

internal interface IStorageProvider
{
    Task<Result<string>> SaveAsync(Stream data, Guid id, CancellationToken ct);
    Task<Result<Stream>> GetAsync(string key, CancellationToken ct);
    Task<Result> DeleteAsync(string key, CancellationToken ct);
}
