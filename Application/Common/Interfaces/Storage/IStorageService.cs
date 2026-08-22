using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Storage
{
    public interface IStorageService
    {
        /// <summary>
        /// رفع ملف إلى MinIO
        /// </summary>
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default);

        /// <summary>
        /// تحميل ملف كـ Stream
        /// </summary>
        Task<Stream> DownloadFileAsync(string fileName, CancellationToken cancellationToken = default);

        /// <summary>
        /// حذف ملف من السيرفر
        /// </summary>
        Task DeleteFileAsync(string fileName, CancellationToken cancellationToken = default);

        /// <summary>
        /// إنشاء رابط مؤقت ومباشر لتحميل/عرض الملف (Presigned URL)
        /// </summary>
        Task<string> GetPresignedUrlAsync(string fileName, int expiryInSeconds = 3600);
    }
}