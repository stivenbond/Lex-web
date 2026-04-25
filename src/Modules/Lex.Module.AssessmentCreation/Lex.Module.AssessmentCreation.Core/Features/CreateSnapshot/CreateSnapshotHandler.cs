using Lex.Module.AssessmentCreation.Core.Domain;

namespace Lex.Module.AssessmentCreation.Core.Features.CreateSnapshot;

public record CreateSnapshotCommand(Guid AssessmentId, string Title, string ContentJson) : IRequest<Result<Guid>>;

internal sealed class CreateSnapshotHandler(IAssessmentRepository repository) 
    : IRequestHandler<CreateSnapshotCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateSnapshotCommand request, CancellationToken ct)
    {
        var snapshot = AssessmentSnapshot.Create(
            request.AssessmentId, 
            request.Title, 
            request.ContentJson);

        repository.AddSnapshot(snapshot);
        await repository.SaveChangesAsync(ct);

        return snapshot.Id;
    }
}
