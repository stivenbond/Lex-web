using Lex.SharedKernel.Primitives;

namespace Lex.Module.LessonManagement.Core.Domain;

public abstract class ContentBlock : ValueObject
{
    public string Type { get; protected init; } = string.Empty;
}
