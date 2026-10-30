using System;
using System.Collections.Generic;

namespace Identity.Contracts.DTOs
{
    /// <summary>Authentication response for the MVC host — same data plus the user Id.</summary>
    public class AuthModelMVC
    {
        public string Message { get; set; } = string.Empty;
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = [];
        public Guid UserId { get; set; }

        /// <summary>Raw JWT issued by the module — the MVC host parks it in the token cookie.</summary>
        public string Token { get; set; } = string.Empty;

        public DateTime Expiration { get; set; }
    }
}
