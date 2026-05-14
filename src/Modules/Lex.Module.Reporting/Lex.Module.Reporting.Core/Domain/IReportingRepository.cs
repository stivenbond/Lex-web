namespace Lex.Module.Reporting.Core.Domain;

public interface IReportingRepository
{
    void AddReport(Report report);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
