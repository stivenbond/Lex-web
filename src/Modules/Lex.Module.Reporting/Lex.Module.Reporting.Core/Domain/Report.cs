using Lex.SharedKernel.Primitives;

namespace Lex.Module.Reporting.Core.Domain;

public sealed class Report : AggregateRoot
{
    public string Title { get; private set; } = string.Empty;
    public string DataJson { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    private Report() { }

    public static Report Create(string title, string dataJson)
    {
        return new Report
        {
            Id = Guid.NewGuid(),
            Title = title,
            DataJson = dataJson,
            CreatedAt = DateTime.UtcNow
        };
    }
}
