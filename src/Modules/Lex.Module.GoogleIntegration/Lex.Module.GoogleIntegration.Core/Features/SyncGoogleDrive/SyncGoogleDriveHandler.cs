using MediatR;
using Lex.SharedKernel.Primitives;
using Lex.SharedKernel.Abstractions;
using Lex.Module.GoogleIntegration.Core.Abstractions;

namespace Lex.Module.GoogleIntegration.Core.Features.SyncGoogleDrive;

public record SyncGoogleDriveCommand(string AccessToken) : IRequest<Result<int>>;

internal sealed class SyncGoogleDriveHandler(
    IGoogleDriveService googleService,
    IObjectStorageService storageService,
    IAsyncJobTracker jobTracker) : IRequestHandler<SyncGoogleDriveCommand, Result<int>>
{
    public async Task<Result<int>> Handle(SyncGoogleDriveCommand request, CancellationToken ct)
    {
        var jobId = Guid.NewGuid();
        await jobTracker.StartJobAsync(jobId, "Google Drive Sync");

        var listResult = await googleService.ListFilesAsync(request.AccessToken, ct);
        if (listResult.IsFailure)
        {
            await jobTracker.FailJobAsync(jobId, listResult.Error!.Message);
            return listResult.Error!;
        }

        var files = listResult.Value!.ToList();
        int count = 0;

        for (int i = 0; i < files.Count; i++)
        {
            var file = files[i];
            int progress = (int)((float)i / files.Count * 100);
            await jobTracker.UpdateProgressAsync(jobId, progress, $"Syncing {file.Name}...");

            var downloadResult = await googleService.DownloadFileAsync(request.AccessToken, file.Id, ct);
            if (downloadResult.IsFailure) continue;

            var uploadResult = await storageService.UploadAsync(
                downloadResult.Value!, 
                file.Name, 
                file.MimeType, 
                ct);

            if (uploadResult.IsSuccess) count++;
        }

        await jobTracker.CompleteJobAsync(jobId);
        return count;
    }
}
