using Lex.SharedKernel.Primitives;

namespace Lex.Module.ImportExport.Core.Domain;

public sealed class ExportJob : AggregateRoot
{
    public string Type { get; private set; } = string.Empty;
    public string Status { get; private set; } = "Pending";
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    private ExportJob() { }

    public static ExportJob Create(string type)
    {
        return new ExportJob
        {
            Id = Guid.NewGuid(),
            Type = type,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };
    }
}
