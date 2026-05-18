using Lex.SharedKernel.Primitives;
using MediatR;

namespace Lex.Module.GoogleIntegration.Core.Features.SyncGoogleDrive;

public sealed record SyncGoogleDriveCommand(string AccessToken) : IRequest<Result<int>>;

