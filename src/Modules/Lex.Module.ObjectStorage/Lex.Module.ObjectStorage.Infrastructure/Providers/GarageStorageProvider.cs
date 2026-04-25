using Lex.SharedKernel.Primitives;
using Minio;
using Minio.DataModel.Args;

namespace Lex.Module.ObjectStorage.Infrastructure.Providers;

internal sealed class GarageStorageProvider(IMinioClient minioClient, string bucketName = "lex-files") : IStorageProvider
{
    public async Task<Result<string>> SaveAsync(Stream data, Guid id, CancellationToken ct)
    {
        try
        {
            var key = id.ToString();
            
            // Ensure bucket exists (simplified for now, usually done at startup)
            var beArgs = new BucketExistsArgs().WithBucket(bucketName);
            bool found = await minioClient.BucketExistsAsync(beArgs, ct);
            if (!found)
            {
                var mbArgs = new MakeBucketArgs().WithBucket(bucketName);
                await minioClient.MakeBucketAsync(mbArgs, ct);
            }

            var putArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(key)
                .WithStreamData(data)
                .WithObjectSize(data.Length);

            await minioClient.PutObjectAsync(putArgs, ct);

            return key;
        }
        catch (Exception ex)
        {
            return Error.Failure("StorageError", $"Failed to save file to Garage: {ex.Message}");
        }
    }

    public async Task<Result<Stream>> GetAsync(string key, CancellationToken ct)
    {
        try
        {
            var ms = new MemoryStream();
            var getArgs = new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(key)
                .WithCallbackStream(s => s.CopyTo(ms));

            await minioClient.GetObjectAsync(getArgs, ct);
            ms.Position = 0;
            return ms;
        }
        catch (Exception ex)
        {
            return Error.Failure("StorageError", $"Failed to retrieve file from Garage: {ex.Message}");
        }
    }

    public async Task<Result> DeleteAsync(string key, CancellationToken ct)
    {
        try
        {
            var rmArgs = new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(key);

            await minioClient.RemoveObjectAsync(rmArgs, ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Error.Failure("StorageError", $"Failed to delete file from Garage: {ex.Message}");
        }
    }
}
