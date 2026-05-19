using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Lex.SharedKernel.Primitives;

namespace Lex.Infrastructure.Persistence;

public abstract class BaseDbContext<TContext>(DbContextOptions<TContext> options) : DbContext(options)
    where TContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.IsOwned()) continue;

            var clrType = entityType.ClrType;
            if (clrType == null) continue;

            if (clrType.GetProperty("CreatedAt") == null)
            {
                modelBuilder.Entity(clrType).Property<DateTimeOffset>("CreatedAt");
            }
            if (clrType.GetProperty("UpdatedAt") == null)
            {
                modelBuilder.Entity(clrType).Property<DateTimeOffset>("UpdatedAt");
            }
        }

        // Apply global conventions
        ApplySnakeCaseNaming(modelBuilder);

        // Apply configurations from the assembly
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void ApplySnakeCaseNaming(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // Table name
            entity.SetTableName(ToSnakeCase(entity.GetTableName() ?? string.Empty));

            // Column names
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(ToSnakeCase(property.GetColumnName()));
            }

            // Keys
            foreach (var key in entity.GetKeys())
            {
                key.SetName(ToSnakeCase(key.GetName() ?? string.Empty));
            }

            foreach (var key in entity.GetForeignKeys())
            {
                key.SetConstraintName(ToSnakeCase(key.GetConstraintName() ?? string.Empty));
            }

            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(ToSnakeCase(index.GetDatabaseName() ?? string.Empty));
            }
        }
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in entries)
        {
            var now = DateTimeOffset.UtcNow;

            if (entry.State == EntityState.Added)
            {
                var createdAtProp = entry.Metadata.FindProperty("CreatedAt");
                if (createdAtProp != null)
                {
                    entry.Property("CreatedAt").CurrentValue = createdAtProp.ClrType == typeof(DateTime)
                        ? now.UtcDateTime
                        : now;
                }
            }

            var updatedAtProp = entry.Metadata.FindProperty("UpdatedAt");
            if (updatedAtProp != null)
            {
                entry.Property("UpdatedAt").CurrentValue = updatedAtProp.ClrType == typeof(DateTime)
                    ? now.UtcDateTime
                    : now;
            }
        }
    }

    private static string ToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        var result = new System.Text.StringBuilder();
        for (var i = 0; i < input.Length; i++)
        {
            var c = input[i];
            if (char.IsUpper(c))
            {
                if (i > 0 && input[i - 1] != '_') result.Append('_');
                result.Append(char.ToLower(c));
            }
            else
            {
                result.Append(c);
            }
        }
        return result.ToString().TrimStart('_');
    }
}
