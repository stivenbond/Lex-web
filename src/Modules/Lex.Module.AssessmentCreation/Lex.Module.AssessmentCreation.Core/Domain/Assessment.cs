using Lex.SharedKernel.Primitives;

namespace Lex.Module.AssessmentCreation.Core.Domain;

public sealed class Assessment : AggregateRoot
{
    public string Title { get; private set; } = string.Empty;
    public string Status { get; private set; } = "Draft";
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    private Assessment() { }

    public static Assessment Create(string title)
    {
        return new Assessment
        {
            Id = Guid.NewGuid(),
            Title = title,
            Status = "Draft",
            CreatedAt = DateTime.UtcNow
        };
    }
}
