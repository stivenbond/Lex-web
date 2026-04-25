namespace Lex.Module.LessonManagement.Core.Domain;

public sealed class TextBlock : ContentBlock
{
    public string Content { get; }

    public TextBlock(string content)
    {
        Type = "Text";
        Content = content;
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Type;
        yield return Content;
    }
}
