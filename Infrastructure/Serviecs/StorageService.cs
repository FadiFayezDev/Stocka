using Application.Common.Interfaces;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Serviecs
{
    public class StorageService : IStorageService
    {
        private readonly BlobContainerClient _container;
        private readonly string _rootPath;
        private readonly string _conteinerName;
        private readonly string? _sasToken;

        public StorageService(IConfiguration config)
        {
            var connectionString = config["AzureBlob:ConnectionString"];
            var containerName = config["AzureBlob:ContainerName"];
            _rootPath = config["AzureBlob:RootPath"] ?? string.Empty;
            _sasToken = config["AzureBlob:SasToken"] ?? null;

            _conteinerName = containerName;

            var blobServiceClient = new BlobServiceClient(connectionString);
            _container = blobServiceClient.GetBlobContainerClient(containerName);

            _container = blobServiceClient.GetBlobContainerClient(containerName);
            _container.CreateIfNotExists();
        }

        public async Task<string> SaveAsync(Stream stream, Guid brandId, Guid productId, string extension)
        {
            var fileName = Path.Combine(brandId.ToString(), GetFileName(productId, extension));
            var blobClient = _container.GetBlobClient(fileName);

            await blobClient.UploadAsync(stream, overwrite: true);

            return blobClient.Uri.ToString();
        }

        public async Task<bool> RemoveAsync(string filePath)
        {
            var fileName = Path.GetRelativePath(Path.Combine(_rootPath, _conteinerName), filePath).Replace('%', '/');
            var blobClient = _container.GetBlobClient(fileName);
            var result = await blobClient.DeleteIfExistsAsync();
            return result.Value;
        }

        public string? GetToken()
            => _sasToken;

        private string SaveFileLocalAsync(Stream stream, Guid brandId, Guid productId, string extension)
        {
            var fileName = ProductImagePath(brandId, productId, extension);

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            using var fileStream = new FileStream(fileName, FileMode.Create);
            stream.CopyTo(fileStream);

            if (!File.Exists(fileName))
            {
                throw new Exception("Failed to save the image.");
            }

            var relativePath = GetRelativePath(fileName);

            if (relativePath == null)
            {
                throw new Exception("Failed to get the relative path of the image.");
            }

            return relativePath;
        }

        private string GetFileName(Guid productId, string extension)
            => $"{productId}{extension}";

        private string GetRelativePath(string fileName)
            => Path.GetRelativePath(Directory.GetCurrentDirectory(), fileName);

        private string MediaFolderAccess()
        {
            var mediaFolder = Path.Combine(Directory.GetCurrentDirectory(), "Media");
            if (!Directory.Exists(mediaFolder))
            {
                Directory.CreateDirectory(mediaFolder);
            }
            return mediaFolder;
        }

        private string BrandFolderAccess(Guid brandId)
        {
            var brandFolder = Path.Combine(MediaFolderAccess(), brandId.ToString());
            if (!Directory.Exists(brandFolder))
            {
                Directory.CreateDirectory(brandFolder);
            }
            return brandFolder;
        }

        private string ProductImagePath(Guid brandId, Guid productId, string extension)
        {
            var brandFolder = BrandFolderAccess(brandId);
            var productImagepath = Path.Combine(brandFolder, GetFileName(productId, extension));
            return productImagepath;
        }
    }
}