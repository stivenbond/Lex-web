using Lex.SharedKernel.Primitives;

namespace Lex.Module.LessonManagement.Core.Domain;

public abstract class ContentBlock : Entity
{
    public string Type { get; protected init; } = string.Empty;
    protected ContentBlock() { }
}
