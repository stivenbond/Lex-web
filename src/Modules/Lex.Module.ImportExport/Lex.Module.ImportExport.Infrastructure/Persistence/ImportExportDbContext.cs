using Microsoft.EntityFrameworkCore;
using Lex.Module.ImportExport.Core.Domain;

namespace Lex.Module.ImportExport.Persistence;

public sealed class ImportExportDbContext : DbContext, IImportExportRepository
{
    public DbSet<ExportJob> ExportJobs => Set<ExportJob>();

    public void AddExportJob(ExportJob job) => ExportJobs.Add(job);
    Task<int> IImportExportRepository.SaveChangesAsync(CancellationToken ct) => base.SaveChangesAsync(ct);

    public ImportExportDbContext(DbContextOptions<ImportExportDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("importexport");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ImportExportDbContext).Assembly);
    }
}
