using Lex.Module.Scheduling.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lex.Module.Scheduling.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the Slot entity.
/// Defines table structure, constraints, and relationships.
/// Slot represents a recurring time period available for teaching.
/// </summary>
public sealed class SlotConfiguration : IEntityTypeConfiguration<Slot>
{
    public void Configure(EntityTypeBuilder<Slot> builder)
    {
        builder.ToTable("slots");

        // Primary key
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        // Foreign key
        builder.Property(x => x.TermId)
            .IsRequired();

        // Properties
        builder.Property(x => x.DayOfWeek)
            .IsRequired()
            .HasComment("0=Sunday, 1=Monday, ..., 6=Saturday");

        builder.Property(x => x.StartTime)
            .IsRequired()
            .HasColumnType("time without time zone");

        builder.Property(x => x.EndTime)
            .IsRequired()
            .HasColumnType("time without time zone");

        builder.Property(x => x.DurationMinutes)
            .IsRequired()
            .HasComment("Duration in minutes (e.g., 45 for 45-minute periods)");

        builder.Property(x => x.SlotNumber)
            .IsRequired()
            .HasComment("Sequential slot number within the day (1, 2, 3, etc.)");

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        builder.Property(x => x.UpdatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        // Relationships
        builder.HasOne(x => x.Term)
            .WithMany(t => t.Slots)
            .HasForeignKey(x => x.TermId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Periods)
            .WithOne(p => p.Slot)
            .HasForeignKey(p => p.SlotId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(x => new { x.TermId, x.DayOfWeek, x.StartTime })
            .IsUnique()
            .HasDatabaseName("ix_unique_slot_per_day_time");

        builder.HasIndex(x => new { x.TermId, x.SlotNumber });
    }
}
