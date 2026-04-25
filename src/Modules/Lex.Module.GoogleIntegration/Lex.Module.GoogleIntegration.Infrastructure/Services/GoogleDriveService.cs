using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Lex.SharedKernel.Primitives;
using Lex.Module.GoogleIntegration.Core.Abstractions;

namespace Lex.Module.GoogleIntegration.Infrastructure.Services;

public sealed class GoogleDriveService : IGoogleDriveService
{
    public async Task<Result<IEnumerable<GoogleFile>>> ListFilesAsync(string accessToken, CancellationToken ct = default)
    {
        try
        {
            var service = GetDriveService(accessToken);
            var request = service.Files.List();
            request.Fields = "files(id, name, mimeType, size)";
            request.Q = "trashed = false";

            var response = await request.ExecuteAsync(ct);
            var files = response.Files.Select(f => new GoogleFile(f.Id, f.Name, f.MimeType, f.Size));

            return Result<IEnumerable<GoogleFile>>.Success(files);
        }
        catch (Exception ex)
        {
            return Error.Failure("GoogleDriveError", ex.Message);
        }
    }

    public async Task<Result<Stream>> DownloadFileAsync(string accessToken, string fileId, CancellationToken ct = default)
    {
        try
        {
            var service = GetDriveService(accessToken);
            var stream = new MemoryStream();
            await service.Files.Get(fileId).DownloadAsync(stream, ct);
            stream.Position = 0;

            return Result<Stream>.Success(stream);
        }
        catch (Exception ex)
        {
            return Error.Failure("GoogleDriveError", ex.Message);
        }
    }

    private static DriveService GetDriveService(string accessToken)
    {
        var credential = GoogleCredential.FromAccessToken(accessToken);
        return new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "Lex Web Platform"
        });
    }
}
