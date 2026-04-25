using Microsoft.EntityFrameworkCore;
using Lex.Infrastructure.Persistence;
using Lex.Module.DiaryManagement.Core.Domain;

namespace Lex.Module.DiaryManagement.Persistence;

public sealed class DiaryDbContext(DbContextOptions<DiaryDbContext> options) 
    : BaseDbContext<DiaryDbContext>(options), IDiaryRepository
{
    public DbSet<DiaryEntry> DiaryEntries => Set<DiaryEntry>();

    public void AddEntry(DiaryEntry entry) => DiaryEntries.Add(entry);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("diary");
        
        modelBuilder.Entity<DiaryEntry>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Content).IsRequired();
            
            builder.OwnsMany(x => x.Attachments, a =>
            {
                a.WithOwner().HasForeignKey("DiaryEntryId");
                a.Property<Guid>("Id");
                a.HasKey("Id");
                a.Property(x => x.FileName).IsRequired().HasMaxLength(255);
                a.ToTable("diary_attachments");
            });
        });

        base.OnModelCreating(modelBuilder);
    }
}
