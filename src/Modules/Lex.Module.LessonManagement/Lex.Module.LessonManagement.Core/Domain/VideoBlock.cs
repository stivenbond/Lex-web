namespace Lex.Module.LessonManagement.Core.Domain;

public sealed class VideoBlock : ContentBlock
{
    public string Url { get; }
    public string Title { get; }

    public VideoBlock(string url, string title)
    {
        Type = "Video";
        Url = url;
        Title = title;
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Type;
        yield return Url;
        yield return Title;
    }
}
