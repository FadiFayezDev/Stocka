using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models
{
    public class LoginModel 
    { 
        public string Email { get; set; } = string.Empty; 
        public string Password { get; set; } = string.Empty;
    }
}