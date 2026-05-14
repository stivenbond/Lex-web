namespace Lex.Module.LessonManagement.Core.Domain;

public sealed class TextBlock : ContentBlock
{
    public string Content { get; private set; } = string.Empty;

    private TextBlock() { }

    public TextBlock(string content)
    {
        Type = "Text";
        Content = content;
    }
}
