using Lex.Module.Scheduling.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lex.Module.Scheduling.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the Section entity.
/// Defines table structure, constraints, and relationships.
/// </summary>
public sealed class SectionConfiguration : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> builder)
    {
        builder.ToTable("sections");

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

        builder.Property(x => x.Grade)
            .HasMaxLength(50)
            .HasComment("Academic level/grade (e.g., 9, 10, 11, 12)");

        builder.Property(x => x.StudentCount)
            .HasComment("Estimated or actual student count in this section");

        builder.Property(x => x.ClassTeacherId)
            .HasMaxLength(256)
            .HasComment("ID or identifier of the class teacher/head teacher");

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
            .WithOne(p => p.Section)
            .HasForeignKey(p => p.SectionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.HasIndex(x => x.Grade);

        builder.HasIndex(x => x.ClassTeacherId);

        builder.HasIndex(x => x.IsActive);
    }
}
