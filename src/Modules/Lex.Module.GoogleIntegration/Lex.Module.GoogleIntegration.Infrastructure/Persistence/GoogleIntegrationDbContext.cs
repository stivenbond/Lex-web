using Microsoft.EntityFrameworkCore;
using Lex.Module.GoogleIntegration.Core.Domain;

namespace Lex.Module.GoogleIntegration.Persistence;

public sealed class GoogleIntegrationDbContext : DbContext, IGoogleRepository
{
    public DbSet<GoogleAccount> Accounts => Set<GoogleAccount>();

    public void AddAccount(GoogleAccount account) => Accounts.Add(account);
    Task<int> IGoogleRepository.SaveChangesAsync(CancellationToken ct) => base.SaveChangesAsync(ct);

    public GoogleIntegrationDbContext(DbContextOptions<GoogleIntegrationDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("googleintegration");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GoogleIntegrationDbContext).Assembly);
    }
}
