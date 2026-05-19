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

        modelBuilder.Entity<ContentBlock>()
            .HasDiscriminator(x => x.Type)
            .HasValue<TextBlock>("Text")
            .HasValue<VideoBlock>("Video");

        modelBuilder.Entity<Lesson>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Description).HasMaxLength(2000);

            builder.HasMany(x => x.Blocks)
                .WithOne()
                .HasForeignKey("LessonId")
                .OnDelete(DeleteBehavior.Cascade);
        });

        base.OnModelCreating(modelBuilder);
    }
}
