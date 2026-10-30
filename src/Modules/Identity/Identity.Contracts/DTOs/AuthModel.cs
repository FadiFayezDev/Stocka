using System;
using System.Collections.Generic;

namespace Identity.Contracts.DTOs
{
    /// <summary>Authentication response carrying the issued JWT.</summary>
    public class AuthModel
    {
        public string Message { get; set; } = string.Empty;
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = [];
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }
}
