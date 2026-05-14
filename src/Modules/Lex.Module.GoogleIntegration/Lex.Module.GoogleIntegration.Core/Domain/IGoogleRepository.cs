namespace Lex.Module.GoogleIntegration.Core.Domain;

public interface IGoogleRepository
{
    void AddAccount(GoogleAccount account);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
