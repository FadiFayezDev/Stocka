using System.ComponentModel.DataAnnotations;

namespace Identity.Contracts.DTOs
{
    /// <summary>
    /// Self-service registration input. The person Party row carries
    /// FirstName/LastName/PhoneNumber (not the user row), so the phone is collected
    /// here too; the organization of the registration is explicit at the call site
    /// (<c>IIdentityService.RegisterAsync(RegisterModel, organizationId)</c>).
    /// </summary>
    public class RegisterModel
    {
        [Required, StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string UserName { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(128)]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(256)]
        public string Password { get; set; } = string.Empty;
    }
}
