using Lex.Module.Reporting.Core.Domain;
using Lex.SharedKernel.Primitives;
using MediatR;

namespace Lex.Module.Reporting.Core.Features.GenerateReport;

public sealed record GenerateReportCommand(string Title) : IRequest<Result<Guid>>;

public sealed class GenerateReportCommandHandler(IReportingRepository repository) : IRequestHandler<GenerateReportCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(GenerateReportCommand request, CancellationToken cancellationToken)
    {
        var report = Report.Create(request.Title, "{}");
        repository.AddReport(report);
        await repository.SaveChangesAsync(cancellationToken);
        return report.Id;
    }
}
