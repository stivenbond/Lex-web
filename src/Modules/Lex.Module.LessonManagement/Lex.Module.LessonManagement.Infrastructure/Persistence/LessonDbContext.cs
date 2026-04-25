using Microsoft.EntityFrameworkCore;
using Lex.Infrastructure.Persistence;
using Lex.Module.LessonManagement.Core.Domain;
using System.Text.Json;

namespace Lex.Module.LessonManagement.Persistence;

public sealed class LessonDbContext(DbContextOptions<LessonDbContext> options) : BaseDbContext<LessonDbContext>(options)
{
    public DbSet<Lesson> Lessons => Set<Lesson>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("lessons");

        modelBuilder.Entity<Lesson>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Description).HasMaxLength(2000);
            
            // Store blocks as JSON for maximum flexibility in this MVP stage
            builder.Property(x => x.Blocks)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<List<ContentBlock>>(v, (JsonSerializerOptions?)null) ?? new List<ContentBlock>())
                .HasColumnType("jsonb");
        });

        base.OnModelCreating(modelBuilder);
    }
}
