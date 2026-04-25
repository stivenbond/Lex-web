using Lex.SharedKernel.Primitives;

namespace Lex.Module.GoogleIntegration.Core.Abstractions;

public interface IGoogleDriveService
{
    Task<Result<IEnumerable<GoogleFile>>> ListFilesAsync(string accessToken, CancellationToken ct = default);
    Task<Result<Stream>> DownloadFileAsync(string accessToken, string fileId, CancellationToken ct = default);
}

public record GoogleFile(string Id, string Name, string MimeType, long? Size);
