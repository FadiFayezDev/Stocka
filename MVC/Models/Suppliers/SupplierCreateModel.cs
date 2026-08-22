using System.ComponentModel.DataAnnotations;

namespace MVC.Models.Suppliers
{
    public class SupplierCreateModel
    {
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Phone]
        public string? Phone { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Address { get; set; }
    }
}
