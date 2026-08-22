using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;

namespace Infrastructure.Storage;

public class StorageService : IStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;
    private readonly string _endpoint;       // الاتصال الداخلي
    private readonly string _publicEndpoint; // للعرض في الـ Frontend
    private readonly bool _useSSL;
    private readonly string? _sasToken;

    public StorageService(IConfiguration config)
    {
        _endpoint = config["Minio:Endpoint"] ?? "localhost:9000";
        _publicEndpoint = config["Minio:PublicEndpoint"] ?? "localhost:9000"; // فصل الـ Public Endpoint

        var accessKey = config["Minio:AccessKey"] ?? config["MINIO_ROOT_USER"] ?? string.Empty;
        var secretKey = config["Minio:SecretKey"] ?? config["MINIO_ROOT_PASSWORD"] ?? string.Empty;
        _bucketName = config["Minio:Bucket"] ?? config["MINIO_DEFAULT_BUCKET"] ?? "stocka";
        _useSSL = bool.TryParse(config["Minio:UseSSL"], out var ssl) && ssl;
        _sasToken = config["Minio:SasToken"] ?? null;

        _minioClient = new MinioClient()
            .WithEndpoint(_endpoint) // يتصل داخلياً بـ minio:9000
            .WithCredentials(accessKey, secretKey);

        if (_useSSL)
        {
            _minioClient.WithSSL();
        }

        _minioClient = _minioClient.Build();

        EnsureBucketExistsAsync().GetAwaiter().GetResult();
    }

    public async Task<string> SaveAsync(Stream stream, Guid brandId, Guid productId, string extension)
    {
        var objectName = $"{brandId}/{GetFileName(productId, extension)}";

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectName)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithContentType(GetContentType(extension));

        await _minioClient.PutObjectAsync(putObjectArgs);

        var protocol = _useSSL ? "https" : "http";

        // استخدام _publicEndpoint بدلاً من _endpoint
        return $"{protocol}://{_publicEndpoint}/{_bucketName}/{objectName}";
    }

    public async Task<bool> RemoveAsync(string fileUrl)
    {
        try
        {
            var uri = new Uri(fileUrl);
            string pathInBucket = uri.AbsolutePath.TrimStart('/');

            string objectName = pathInBucket;
            if (pathInBucket.StartsWith(_bucketName + "/", StringComparison.OrdinalIgnoreCase))
            {
                objectName = pathInBucket.Substring(_bucketName.Length + 1);
            }

            objectName = Uri.UnescapeDataString(objectName).Replace('\\', '/');

            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName);

            await _minioClient.RemoveObjectAsync(removeObjectArgs);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public string? GetToken() => _sasToken;

    private async Task EnsureBucketExistsAsync()
    {
        var bucketExistsArgs = new BucketExistsArgs().WithBucket(_bucketName);
        bool found = await _minioClient.BucketExistsAsync(bucketExistsArgs);

        if (!found)
        {
            var makeBucketArgs = new MakeBucketArgs().WithBucket(_bucketName);
            await _minioClient.MakeBucketAsync(makeBucketArgs);
        }
    }

    private string GetContentType(string extension)
    {
        return extension.ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            ".pdf" => "application/pdf",
            _ => "application/octet-stream"
        };
    }

    private string GetFileName(Guid productId, string extension)
        => $"{productId}{extension}";
}