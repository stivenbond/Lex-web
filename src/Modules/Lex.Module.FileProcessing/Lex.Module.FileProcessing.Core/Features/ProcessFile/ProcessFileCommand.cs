using Lex.Module.FileProcessing.Core.Domain;
using Lex.SharedKernel.Primitives;
using MediatR;

namespace Lex.Module.FileProcessing.Core.Features.ProcessFile;

public sealed record ProcessFileCommand(string FileName) : IRequest<Result<Guid>>;

public sealed class ProcessFileCommandHandler(IFileRepository repository) : IRequestHandler<ProcessFileCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(ProcessFileCommand request, CancellationToken cancellationToken)
    {
        var job = FileJob.Create(request.FileName);
        repository.AddJob(job);
        await repository.SaveChangesAsync(cancellationToken);
        return job.Id;
    }
}
