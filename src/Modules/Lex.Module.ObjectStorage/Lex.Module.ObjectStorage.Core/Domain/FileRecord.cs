using Lex.SharedKernel.Primitives;

namespace Lex.Module.ObjectStorage.Core.Domain;

public sealed class FileRecord : AggregateRoot
{
    public string FileName { get; private set; } = string.Empty;
    public string ContentType { get; private set; } = string.Empty;
    public long SizeBytes { get; private set; }
    public StorageProvider Provider { get; private set; }
    public string ProviderKey { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    private FileRecord() { } // EF Core

    public static FileRecord Create(string fileName, string contentType, long sizeBytes, StorageProvider provider, string providerKey)
    {
        return new FileRecord
        {
            Id = Guid.NewGuid(),
            FileName = fileName,
            ContentType = contentType,
            SizeBytes = sizeBytes,
            Provider = provider,
            ProviderKey = providerKey,
            CreatedAt = DateTime.UtcNow
        };
    }
}
