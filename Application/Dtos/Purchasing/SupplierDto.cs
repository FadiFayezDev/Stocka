using System;

namespace Application.Dtos.Purchasing
{
    public class SupplierDto
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}
