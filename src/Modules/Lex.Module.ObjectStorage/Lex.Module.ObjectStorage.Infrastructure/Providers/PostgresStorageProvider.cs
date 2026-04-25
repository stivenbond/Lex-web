using Lex.SharedKernel.Primitives;
using Lex.Module.ObjectStorage.Persistence;
using Lex.Module.ObjectStorage.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Lex.Module.ObjectStorage.Infrastructure.Providers;

internal sealed class PostgresStorageProvider(ObjectStorageDbContext dbContext) : IStorageProvider
{
    public async Task<Result<string>> SaveAsync(Stream data, Guid id, CancellationToken ct)
    {
        using var ms = new MemoryStream();
        await data.CopyToAsync(ms, ct);
        
        var storedFile = new StoredFile { Id = id, Data = ms.ToArray() };
        dbContext.StoredFiles.Add(storedFile);
        
        return id.ToString(); // The key is just the ID
    }

    public async Task<Result<Stream>> GetAsync(string key, CancellationToken ct)
    {
        if (!Guid.TryParse(key, out var id)) 
            return Error.Validation("InvalidKey", "The key must be a valid GUID for Postgres storage.");

        var storedFile = await dbContext.StoredFiles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
        if (storedFile == null) 
            return Error.NotFound("FileNotFound", "File content not found in Postgres storage.");

        return new MemoryStream(storedFile.Data);
    }

    public async Task<Result> DeleteAsync(string key, CancellationToken ct)
    {
        if (!Guid.TryParse(key, out var id)) 
            return Error.Validation("InvalidKey", "The key must be a valid GUID for Postgres storage.");

        await dbContext.StoredFiles.Where(x => x.Id == id).ExecuteDeleteAsync(ct);
        return Result.Success();
    }
}
