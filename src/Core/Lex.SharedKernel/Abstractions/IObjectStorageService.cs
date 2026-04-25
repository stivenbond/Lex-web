using Lex.SharedKernel.Primitives;

namespace Lex.SharedKernel.Abstractions;

public interface IObjectStorageService
{
    Task<Result<Guid>> UploadAsync(Stream data, string fileName, string contentType, CancellationToken ct = default);
    Task<Result<Stream>> DownloadAsync(Guid fileId, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid fileId, CancellationToken ct = default);
    Task<Result<IEnumerable<FileMetadata>>> ListFilesAsync(CancellationToken ct = default);
}

public record FileMetadata(Guid Id, string FileName, string ContentType, long Size, DateTimeOffset CreatedAt);
