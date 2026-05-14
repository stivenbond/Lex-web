namespace Lex.Module.AssessmentCreation.Core.Domain;

public interface IAssessmentRepository
{
    void AddSnapshot(AssessmentSnapshot snapshot);
    void AddAssessment(Assessment assessment);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
