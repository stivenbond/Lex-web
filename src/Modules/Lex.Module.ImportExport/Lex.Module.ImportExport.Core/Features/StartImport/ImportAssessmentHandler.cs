using MediatR;
using Lex.SharedKernel.Primitives;
using Lex.SharedKernel.Abstractions;

namespace Lex.Module.ImportExport.Core.Features.StartImport;

public record ImportAssessmentCommand(Guid FileId, string FileName) : IRequest<Result<Guid>>;

internal sealed class ImportAssessmentHandler(
    IObjectStorageService storageService,
    IAsyncJobTracker jobTracker) : IRequestHandler<ImportAssessmentCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(ImportAssessmentCommand request, CancellationToken ct)
    {
        var jobId = Guid.NewGuid();
        await jobTracker.StartJobAsync(jobId, $"Importing {request.FileName}");

        var downloadResult = await storageService.DownloadAsync(request.FileId, ct);
        if (downloadResult.IsFailure)
        {
            await jobTracker.FailJobAsync(jobId, "File not found.");
            return downloadResult.Error!;
        }

        await jobTracker.UpdateProgressAsync(jobId, 30, "Analyzing document structure...");
        await Task.Delay(1000, ct); // Simulate processing

        await jobTracker.UpdateProgressAsync(jobId, 60, "Extracting questions and metadata...");
        await Task.Delay(1000, ct); // Simulate processing

        await jobTracker.UpdateProgressAsync(jobId, 90, "Finalizing import...");
        await Task.Delay(500, ct);

        await jobTracker.CompleteJobAsync(jobId);
        return Guid.NewGuid(); // Mock imported assessment ID
    }
}
