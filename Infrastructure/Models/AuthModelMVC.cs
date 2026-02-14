using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models
{
    public class AuthModelMVC
    {
        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public Guid UserId { get; set; }
        public DateTime Expiration { get; set; }
    }
}
