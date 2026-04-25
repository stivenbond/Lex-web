using Lex.SharedKernel.Primitives;
using Lex.SharedKernel.Abstractions;
using Lex.Module.ObjectStorage.Core.Domain;
using Lex.Module.ObjectStorage.Persistence;
using Lex.Module.ObjectStorage.Infrastructure.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Lex.Module.ObjectStorage.Infrastructure;

internal sealed class ObjectStorageService(
    ObjectStorageDbContext dbContext,
    IEnumerable<IStorageProvider> providers,
    IConfiguration configuration) : IObjectStorageService
{
    private const long PostgresLimit = 10 * 1024 * 1024; // 10 MB

    public async Task<Result<Guid>> UploadAsync(Stream data, string fileName, string contentType, CancellationToken ct)
    {
        var providerType = GetConfiguredProvider();
        
        if (providerType == StorageProvider.PostgreSQL && data.Length > PostgresLimit)
        {
            return Error.Validation("FileTooLarge", "Files larger than 10MB cannot be stored in PostgreSQL. Please switch to Garage storage.");
        }

        var provider = GetProvider(providerType);
        var id = Guid.NewGuid();

        var saveResult = await provider.SaveAsync(data, id, ct);
        if (saveResult.IsFailure) return saveResult.Error!;

        var record = FileRecord.Create(fileName, contentType, data.Length, providerType, saveResult.Value!);
        dbContext.FileRecords.Add(record);
        
        await dbContext.SaveChangesAsync(ct);
        
        return record.Id;
    }

    public async Task<Result<Stream>> DownloadAsync(Guid fileId, CancellationToken ct)
    {
        var record = await dbContext.FileRecords.AsNoTracking().FirstOrDefaultAsync(x => x.Id == fileId, ct);
        if (record == null) return Error.NotFound("FileNotFound", "File record not found.");

        var provider = GetProvider(record.Provider);
        return await provider.GetAsync(record.ProviderKey, ct);
    }

    public async Task<Result> DeleteAsync(Guid fileId, CancellationToken ct)
    {
        var record = await dbContext.FileRecords.FirstOrDefaultAsync(x => x.Id == fileId, ct);
        if (record == null) return Error.NotFound("FileNotFound", "File record not found.");

        var provider = GetProvider(record.Provider);
        var deleteResult = await provider.DeleteAsync(record.ProviderKey, ct);
        if (deleteResult.IsFailure) return deleteResult.Error!;

        dbContext.FileRecords.Remove(record);
        await dbContext.SaveChangesAsync(ct);
        
        return Result.Success();
    }

    public async Task<Result<IEnumerable<FileMetadata>>> ListFilesAsync(CancellationToken ct = default)
    {
        var files = await dbContext.FileRecords
            .AsNoTracking()
            .Select(x => new FileMetadata(x.Id, x.FileName, x.ContentType, x.SizeBytes, x.CreatedAt))
            .ToListAsync(ct);

        return files;
    }

    private StorageProvider GetConfiguredProvider()
    {
        var providerStr = configuration["ObjectStorage:Provider"] ?? "Garage";
        return Enum.TryParse<StorageProvider>(providerStr, true, out var p) ? p : StorageProvider.Garage;
    }

    private IStorageProvider GetProvider(StorageProvider type)
    {
        return type switch
        {
            StorageProvider.PostgreSQL => providers.OfType<PostgresStorageProvider>().First(),
            StorageProvider.Garage => providers.OfType<GarageStorageProvider>().First(),
            _ => throw new NotSupportedException($"Provider {type} is not supported.")
        };
    }
}
