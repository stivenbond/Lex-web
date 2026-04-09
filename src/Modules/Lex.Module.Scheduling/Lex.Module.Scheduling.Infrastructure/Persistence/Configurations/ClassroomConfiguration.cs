using Lex.Module.Scheduling.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lex.Module.Scheduling.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the Classroom entity.
/// Defines table structure, constraints, and relationships.
/// </summary>
public sealed class ClassroomConfiguration : IEntityTypeConfiguration<Classroom>
{
    public void Configure(EntityTypeBuilder<Classroom> builder)
    {
        builder.ToTable("classrooms");

        // Primary key
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        // Properties
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Property(x => x.Capacity)
            .HasComment("Number of students the classroom can accommodate");

        builder.Property(x => x.Facilities)
            .HasMaxLength(500)
            .HasComment("Comma-separated list of facilities (e.g., Projector, Whiteboard)");

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        builder.Property(x => x.UpdatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        // Relationships
        builder.HasMany(x => x.Periods)
            .WithOne(p => p.Classroom)
            .HasForeignKey(p => p.ClassroomId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.HasIndex(x => x.IsActive);
    }
}
