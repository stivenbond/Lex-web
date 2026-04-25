using Lex.SharedKernel.Primitives;

namespace Lex.Module.AssessmentCreation.Core.Domain;

public sealed class AssessmentSnapshot : AggregateRoot
{
    public Guid OriginalAssessmentId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string ContentJson { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    private AssessmentSnapshot() { }

    public static AssessmentSnapshot Create(Guid originalId, string title, string contentJson)
    {
        return new AssessmentSnapshot
        {
            Id = Guid.NewGuid(),
            OriginalAssessmentId = originalId,
            Title = title,
            ContentJson = contentJson,
            CreatedAt = DateTime.UtcNow
        };
    }
}
