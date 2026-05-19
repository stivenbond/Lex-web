using Lex.Module.AssessmentCreation.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Lex.Module.AssessmentCreation.Persistence;

public sealed class AssessmentCreationDbContext : DbContext, IAssessmentRepository
{
    public DbSet<AssessmentSnapshot> Snapshots => Set<AssessmentSnapshot>();
    public DbSet<Assessment> Assessments => Set<Assessment>();

    public void AddSnapshot(AssessmentSnapshot snapshot) => Snapshots.Add(snapshot);
    public void AddAssessment(Assessment assessment) => Assessments.Add(assessment);
    Task<int> IAssessmentRepository.SaveChangesAsync(CancellationToken ct) => base.SaveChangesAsync(ct);

    public AssessmentCreationDbContext(DbContextOptions<AssessmentCreationDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("assessmentcreation");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssessmentCreationDbContext).Assembly);
    }
}
