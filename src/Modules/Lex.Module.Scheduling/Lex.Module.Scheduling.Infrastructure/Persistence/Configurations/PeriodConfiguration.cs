using Lex.Module.Scheduling.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lex.Module.Scheduling.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the Period entity.
/// Defines table structure, constraints, and relationships.
/// Period represents an actual teaching assignment: teacher + section + subject + classroom in a specific time slot.
/// </summary>
public sealed class PeriodConfiguration : IEntityTypeConfiguration<Period>
{
    public void Configure(EntityTypeBuilder<Period> builder)
    {
        builder.ToTable("periods");

        // Primary key
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        // Foreign keys
        builder.Property(x => x.SlotId)
            .IsRequired();

        builder.Property(x => x.SectionId)
            .IsRequired();

        builder.Property(x => x.ClassroomId)
            .IsRequired();

        // Properties
        builder.Property(x => x.Subject)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.TeacherId)
            .IsRequired()
            .HasMaxLength(256)
            .HasComment("ID of the teacher assigned to teach this period (reference to identity system)");

        builder.Property(x => x.TeacherName)
            .HasMaxLength(200)
            .HasComment("Cached display name of the teacher for performance/readability");

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(x => x.Notes)
            .HasMaxLength(500)
            .HasComment("Optional notes (e.g., Lab session, Practical assessment)");

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        builder.Property(x => x.UpdatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        // Relationships - required navigation properties
        builder.HasOne(x => x.Slot)
            .WithMany(s => s.Periods)
            .HasForeignKey(x => x.SlotId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(x => x.Section)
            .WithMany(se => se.Periods)
            .HasForeignKey(x => x.SectionId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(x => x.Classroom)
            .WithMany(c => c.Periods)
            .HasForeignKey(x => x.ClassroomId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        // Indexes
        builder.HasIndex(x => new { x.SlotId, x.SectionId })
            .IsUnique()
            .HasDatabaseName("ix_unique_period_per_section_slot");

        builder.HasIndex(x => new { x.SlotId, x.ClassroomId })
            .IsUnique()
            .HasDatabaseName("ix_unique_period_per_classroom_slot");

        builder.HasIndex(x => new { x.SlotId, x.TeacherId })
            .IsUnique()
            .HasDatabaseName("ix_unique_period_per_teacher_slot");

        builder.HasIndex(x => x.Subject);

        builder.HasIndex(x => x.TeacherId);

        builder.HasIndex(x => x.IsActive);
    }
}
