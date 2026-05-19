using Lex.Module.ImportExport.Core.Domain;
using Lex.SharedKernel.Primitives;
using MediatR;

namespace Lex.Module.ImportExport.Core.Features.CreateExport;

public sealed record CreateExportCommand(string Type) : IRequest<Result<Guid>>;

internal sealed class CreateExportCommandHandler(IImportExportRepository repository) : IRequestHandler<CreateExportCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateExportCommand request, CancellationToken cancellationToken)
    {
        var job = ExportJob.Create(request.Type);
        repository.AddExportJob(job);
        await repository.SaveChangesAsync(cancellationToken);
        return job.Id;
    }
}
