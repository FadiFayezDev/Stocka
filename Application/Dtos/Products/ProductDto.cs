using System;

namespace Application.Dtos.Products
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string? Barcode { get; set; }
    }
}
