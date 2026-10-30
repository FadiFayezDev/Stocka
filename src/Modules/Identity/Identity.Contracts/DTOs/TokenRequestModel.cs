using System.ComponentModel.DataAnnotations;

namespace Identity.Contracts.DTOs
{
    /// <summary>MVC login-by-email request (issues a JWT).</summary>
    public class TokenRequestModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
