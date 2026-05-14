namespace Lex.Module.LessonManagement.Core.Domain;

public sealed class VideoBlock : ContentBlock
{
    public string Url { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;

    private VideoBlock() { }

    public VideoBlock(string url, string title)
    {
        Type = "Video";
        Url = url;
        Title = title;
    }
}
