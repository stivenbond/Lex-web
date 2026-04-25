using Lex.SharedKernel.Primitives;

namespace Lex.Module.AssessmentDelivery.Core.Domain;

public sealed class AssessmentSession : AggregateRoot
{
    public Guid SnapshotId { get; private set; }
    public string StudentId { get; private set; } = string.Empty;
    public DateTime StartTime { get; private set; }
    public DateTime? EndTime { get; private set; }
    private readonly List<Answer> _answers = new();
    public IReadOnlyCollection<Answer> Answers => _answers.AsReadOnly();
    public SessionStatus Status { get; private set; }

    private AssessmentSession() { }

    public static AssessmentSession Start(Guid snapshotId, string studentId)
    {
        return new AssessmentSession
        {
            Id = Guid.NewGuid(),
            SnapshotId = snapshotId,
            StudentId = studentId,
            StartTime = DateTime.UtcNow,
            Status = SessionStatus.InProgress
        };
    }

    public void SaveAnswer(string questionId, string content)
    {
        if (Status != SessionStatus.InProgress) return;
        
        var existing = _answers.FirstOrDefault(a => a.QuestionId == questionId);
        if (existing != null) _answers.Remove(existing);
        
        _answers.Add(new Answer(questionId, content));
    }

    public void Submit()
    {
        if (Status != SessionStatus.InProgress) return;
        Status = SessionStatus.Submitted;
        EndTime = DateTime.UtcNow;
    }
}

public class Answer(string QuestionId, string Content) : ValueObject
{
    public string QuestionId { get; } = QuestionId;
    public string Content { get; } = Content;

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return QuestionId;
        yield return Content;
    }
}

public enum SessionStatus
{
    InProgress,
    Submitted,
    Graded
}
