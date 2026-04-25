using Lex.SharedKernel.Primitives;

namespace Lex.Module.DiaryManagement.Core.Domain;

public sealed class DiaryEntry : AggregateRoot
{
    public string Title { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public DateTime Date { get; private set; }
    private readonly List<DiaryAttachment> _attachments = new();
    public IReadOnlyCollection<DiaryAttachment> Attachments => _attachments.AsReadOnly();

    private DiaryEntry() { }

    public static DiaryEntry Create(string title, string content, DateTime date)
    {
        return new DiaryEntry
        {
            Id = Guid.NewGuid(),
            Title = title,
            Content = content,
            Date = date
        };
    }

    public void AddAttachment(Guid fileId, string fileName)
    {
        _attachments.Add(new DiaryAttachment(fileId, fileName));
    }
}
