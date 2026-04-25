namespace Lex.Module.AssessmentCreation.Core.Domain;

public interface IAssessmentRepository
{
    void AddSnapshot(AssessmentSnapshot snapshot);
    Task SaveChangesAsync(CancellationToken ct = default);
}
