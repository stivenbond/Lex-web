using Lex.Module.Scheduling.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lex.Module.Scheduling.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the Term entity.
/// Defines table structure, constraints, and relationships.
/// </summary>
public sealed class TermConfiguration : IEntityTypeConfiguration<Term>
{
    public void Configure(EntityTypeBuilder<Term> builder)
    {
        builder.ToTable("terms");

        // Primary key
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        // Foreign key
        builder.Property(x => x.AcademicYearId)
            .IsRequired();

        // Properties
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.SequenceNumber)
            .IsRequired();

        builder.Property(x => x.StartDate)
            .IsRequired()
            .HasColumnType("date");

        builder.Property(x => x.EndDate)
            .IsRequired()
            .HasColumnType("date");

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        builder.Property(x => x.UpdatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        // Relationships
        builder.HasOne(x => x.AcademicYear)
            .WithMany(a => a.Terms)
            .HasForeignKey(x => x.AcademicYearId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Slots)
            .WithOne(s => s.Term)
            .HasForeignKey(s => s.TermId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(x => new { x.AcademicYearId, x.SequenceNumber })
            .IsUnique();

        builder.HasIndex(x => x.IsActive);
    }
}
