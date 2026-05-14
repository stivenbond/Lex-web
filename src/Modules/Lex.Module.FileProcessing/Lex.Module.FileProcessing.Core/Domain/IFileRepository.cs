namespace Lex.Module.FileProcessing.Core.Domain;

public interface IFileRepository
{
    void AddJob(FileJob job);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
