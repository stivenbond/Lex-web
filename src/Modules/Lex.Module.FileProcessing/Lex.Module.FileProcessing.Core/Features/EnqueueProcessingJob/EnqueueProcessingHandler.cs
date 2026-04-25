using MediatR;
using Lex.SharedKernel.Primitives;
using Lex.SharedKernel.Abstractions;
using Lex.Module.FileProcessing.Core.Abstractions;

namespace Lex.Module.FileProcessing.Core.Features.EnqueueProcessingJob;

public record EnqueueProcessingCommand(Guid FileId, string FileName) : IRequest<Result<Guid>>;

internal sealed class EnqueueProcessingHandler(
    IObjectStorageService storageService,
    IEnumerable<IFileProcessor> processors,
    IAsyncJobTracker jobTracker) : IRequestHandler<EnqueueProcessingCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(EnqueueProcessingCommand request, CancellationToken ct)
    {
        var jobId = Guid.NewGuid();
        var extension = System.IO.Path.GetExtension(request.FileName).ToLower();
        var processor = processors.FirstOrDefault(p => p.SupportedExtension == extension);

        if (processor == null)
        {
            return Error.Validation("UnsupportedFile", $"File extension {extension} is not supported.");
        }

        // We run the processing in the background. In a real scenario, this would be a Message Queue consumer.
        // For this implementation, we'll simulate the background job.
        _ = Task.Run(async () =>
        {
            await jobTracker.StartJobAsync(jobId, $"Processing {request.FileName}");
            
            await jobTracker.UpdateProgressAsync(jobId, 10, "Downloading file...");
            var downloadResult = await storageService.DownloadAsync(request.FileId, ct);
            if (downloadResult.IsFailure)
            {
                await jobTracker.FailJobAsync(jobId, downloadResult.Error!.Message);
                return;
            }

            await jobTracker.UpdateProgressAsync(jobId, 30, "Extracting text...");
            var extractResult = await processor.ExtractTextAsync(downloadResult.Value!, ct);
            if (extractResult.IsFailure)
            {
                await jobTracker.FailJobAsync(jobId, extractResult.Error!.Message);
                return;
            }

            await jobTracker.UpdateProgressAsync(jobId, 90, "Finalizing...");
            // Here you would save the extracted text to a DB or search index
            
            await jobTracker.CompleteJobAsync(jobId);
        }, ct);

        return jobId;
    }
}
