using Lex.Module.GoogleIntegration.Core.Domain;
using Lex.SharedKernel.Primitives;
using MediatR;

namespace Lex.Module.GoogleIntegration.Core.Features.SyncGoogleDrive;

public sealed record SyncGoogleDriveCommand(string AccessToken) : IRequest<Result<int>>;

public sealed class SyncGoogleDriveCommandHandler(IGoogleRepository repository) : IRequestHandler<SyncGoogleDriveCommand, Result<int>>
{
    public async Task<Result<int>> Handle(SyncGoogleDriveCommand request, CancellationToken cancellationToken)
    {
        var account = GoogleAccount.Create("user123", "test@google.com", request.AccessToken);
        repository.AddAccount(account);
        await repository.SaveChangesAsync(cancellationToken);
        return 1; // Return count of synced items
    }
}
