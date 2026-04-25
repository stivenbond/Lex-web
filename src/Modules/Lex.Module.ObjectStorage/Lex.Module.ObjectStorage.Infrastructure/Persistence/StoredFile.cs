namespace Lex.Module.ObjectStorage.Infrastructure.Persistence;

public sealed class StoredFile
{
    public Guid Id { get; set; }
    public byte[] Data { get; set; } = [];
}
