namespace Lex.Module.DiaryManagement.Core.Domain;

public interface IDiaryRepository
{
    void AddEntry(DiaryEntry entry);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
