using Microsoft.EntityFrameworkCore;
using Lex.Module.Reporting.Core.Domain;

namespace Lex.Module.Reporting.Persistence;

public sealed class ReportingDbContext : DbContext, IReportingRepository
{
    public DbSet<Report> Reports => Set<Report>();

    public void AddReport(Report report) => Reports.Add(report);
    Task<int> IReportingRepository.SaveChangesAsync(CancellationToken ct) => base.SaveChangesAsync(ct);

    public ReportingDbContext(DbContextOptions<ReportingDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("reporting");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReportingDbContext).Assembly);
    }
}
