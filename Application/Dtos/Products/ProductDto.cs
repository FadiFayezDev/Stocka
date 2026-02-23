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
        public decimal SellingPrice { get; set; }
        public int TotalQuantity { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
    }
}