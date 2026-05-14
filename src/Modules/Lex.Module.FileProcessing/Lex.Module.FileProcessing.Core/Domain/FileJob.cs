using Lex.SharedKernel.Primitives;

namespace Lex.Module.FileProcessing.Core.Domain;

public sealed class FileJob : AggregateRoot
{
    public string FileName { get; private set; } = string.Empty;
    public string Status { get; private set; } = "Pending";
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    private FileJob() { }

    public static FileJob Create(string fileName)
    {
        return new FileJob
        {
            Id = Guid.NewGuid(),
            FileName = fileName,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };
    }
}
