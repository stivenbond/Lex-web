namespace Lex.Module.AssessmentCreation.Core.Domain;

public interface IAssessmentRepository
{
    void AddSnapshot(AssessmentSnapshot snapshot);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
