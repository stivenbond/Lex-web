using Microsoft.EntityFrameworkCore;
using Lex.Module.FileProcessing.Core.Domain;

namespace Lex.Module.FileProcessing.Persistence;

public sealed class FileProcessingDbContext : DbContext, IFileRepository
{
    public DbSet<FileJob> Jobs => Set<FileJob>();

    public void AddJob(FileJob job) => Jobs.Add(job);
    Task<int> IFileRepository.SaveChangesAsync(CancellationToken ct) => base.SaveChangesAsync(ct);
    public FileProcessingDbContext(DbContextOptions<FileProcessingDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("fileprocessing");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FileProcessingDbContext).Assembly);
    }
}
