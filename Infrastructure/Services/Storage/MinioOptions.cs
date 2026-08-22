using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services.Storage
{
    public class MinioOptions
    {
        public const string SectionName = "Minio";

        public string Endpoint { get; set; } = string.Empty;
        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string Bucket { get; set; } = string.Empty;
        public bool UseSSL { get; set; } = false;
    }
}
