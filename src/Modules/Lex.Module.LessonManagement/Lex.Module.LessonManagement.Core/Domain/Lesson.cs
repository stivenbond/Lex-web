using Lex.SharedKernel.Primitives;

namespace Lex.Module.LessonManagement.Core.Domain;

public sealed class Lesson : AggregateRoot
{
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    private readonly List<ContentBlock> _blocks = new();
    public IReadOnlyCollection<ContentBlock> Blocks => _blocks.AsReadOnly();

    private Lesson() { }

    public static Lesson Create(string title, string description)
    {
        return new Lesson
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description
        };
    }

    public void AddBlock(ContentBlock block)
    {
        _blocks.Add(block);
    }
}

public abstract class ContentBlock : ValueObject
{
    public string Type { get; protected set; } = string.Empty;
}

public class TextBlock(string Content) : ContentBlock
{
    public string Content { get; } = Content;
    public new string Type => "Text";

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Type;
        yield return Content;
    }
}

public class VideoBlock(string Url, string Title) : ContentBlock
{
    public string Url { get; } = Url;
    public string Title { get; } = Title;
    public new string Type => "Video";

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Type;
        yield return Url;
        yield return Title;
    }
}
