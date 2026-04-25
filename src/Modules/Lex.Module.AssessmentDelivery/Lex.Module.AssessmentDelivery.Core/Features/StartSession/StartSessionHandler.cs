using Lex.Module.AssessmentDelivery.Core.Domain;

namespace Lex.Module.AssessmentDelivery.Core.Features.StartSession;

public record StartSessionCommand(Guid SnapshotId, string StudentId) : IRequest<Result<Guid>>;

internal sealed class StartSessionHandler(IDeliveryRepository repository) 
    : IRequestHandler<StartSessionCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(StartSessionCommand request, CancellationToken ct)
    {
        var session = AssessmentSession.Start(request.SnapshotId, request.StudentId);

        repository.AddSession(session);
        await repository.SaveChangesAsync(ct);

        return session.Id;
    }
}
