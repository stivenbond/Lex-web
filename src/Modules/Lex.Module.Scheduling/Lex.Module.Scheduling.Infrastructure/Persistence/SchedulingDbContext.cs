using Lex.Module.Scheduling.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Lex.Module.Scheduling.Persistence;

/// <summary>
/// DbContext for the Scheduling module.
/// Manages the "scheduling" schema and all scheduling-related aggregates.
/// This module is responsible for its own schema and can be extracted into a microservice independently.
/// </summary>
public sealed class SchedulingDbContext : DbContext
{
    public SchedulingDbContext(DbContextOptions<SchedulingDbContext> options) : base(options) { }

    /// <summary>
    /// Academic years - top-level container for all scheduling activities.
    /// </summary>
    public DbSet<AcademicYear> AcademicYears { get; set; } = null!;

    /// <summary>
    /// Terms - divisions within an academic year (e.g., Fall, Winter, Spring).
    /// </summary>
    public DbSet<Term> Terms { get; set; } = null!;

    /// <summary>
    /// Slots - recurring time periods available for teaching (e.g., Monday 08:00–08:45).
    /// </summary>
    public DbSet<Slot> Slots { get; set; } = null!;

    /// <summary>
    /// Periods - actual teaching assignments (lesson/class sessions).
    /// A period is the concrete assignment of teacher, section, subject, and classroom to a slot.
    /// </summary>
    public DbSet<Period> Periods { get; set; } = null!;

    /// <summary>
    /// Classrooms - physical locations where teaching occurs.
    /// </summary>
    public DbSet<Classroom> Classrooms { get; set; } = null!;

    /// <summary>
    /// Sections - student groups/classes (e.g., "9A", "10B-Science").
    /// </summary>
    public DbSet<Section> Sections { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Set default schema for this module
        modelBuilder.HasDefaultSchema("scheduling");

        // Apply all IEntityTypeConfiguration<T> implementations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SchedulingDbContext).Assembly);

        // Ensure database is created with proper constraints
        base.OnModelCreating(modelBuilder);
    }
}

