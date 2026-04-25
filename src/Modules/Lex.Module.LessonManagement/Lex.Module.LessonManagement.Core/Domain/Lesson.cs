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
