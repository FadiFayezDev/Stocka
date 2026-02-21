using Application.Common.Interfaces;
using Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Infrastructure.Serviecs
{
    public class ImageStorageService : IImageStorageService
    {
        public async Task<string> SaveAsync(Stream stream, Guid brandId, Guid productId, string extension)
        {
            var fileName = ProductImagePath(brandId, productId, extension);

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            using var fileStream = new FileStream(fileName, FileMode.Create);
            await stream.CopyToAsync(fileStream);

            if (!File.Exists(fileName))
            {
                throw new Exception("Failed to save the image.");
            }

            var relativePath = GetRelativePath(fileName);

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