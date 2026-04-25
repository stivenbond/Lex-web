using Lex.Module.ObjectStorage.Core.Domain;
using Lex.Module.ObjectStorage.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Lex.Module.ObjectStorage.Persistence;

public sealed class ObjectStorageDbContext : DbContext
{
    public ObjectStorageDbContext(DbContextOptions<ObjectStorageDbContext> options) : base(options) { }

    public DbSet<FileRecord> FileRecords => Set<FileRecord>();
    public DbSet<StoredFile> StoredFiles => Set<StoredFile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("objectstorage");
        
        modelBuilder.Entity<FileRecord>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.FileName).IsRequired().HasMaxLength(255);
            builder.Property(x => x.ContentType).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Provider).IsRequired();
            builder.Property(x => x.ProviderKey).IsRequired().HasMaxLength(500);
        });

        modelBuilder.Entity<StoredFile>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Data).IsRequired();
        });
    }
}
