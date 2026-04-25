using Lex.Module.AssessmentCreation.Core.Domain;

namespace Lex.Module.AssessmentCreation.Persistence;
public sealed class AssessmentCreationDbContext : DbContext, IAssessmentRepository
{
    public DbSet<AssessmentSnapshot> Snapshots => Set<AssessmentSnapshot>();

    public void AddSnapshot(AssessmentSnapshot snapshot) => Snapshots.Add(snapshot);

    public AssessmentCreationDbContext(DbContextOptions<AssessmentCreationDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("assessmentcreation");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssessmentCreationDbContext).Assembly);
    }
}
