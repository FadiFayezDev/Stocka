using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces
{
    public interface IStorageService
    {
        Task<string> SaveAsync(Stream stream, Guid brandId, Guid productId, string extension);
        Task<bool> RemoveAsync(string filePath);
        string? GetToken();
    }
}
