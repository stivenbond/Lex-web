using Lex.SharedKernel.Primitives;

namespace Lex.Module.AssessmentDelivery.Core.Domain;

public sealed class Answer(string questionId, string content) : ValueObject
{
    public string QuestionId { get; } = questionId;
    public string Content { get; } = content;

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return QuestionId;
        yield return Content;
    }
}
