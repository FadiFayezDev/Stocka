using System;

namespace Application.Dtos.Products
{
    public class ProductCategoryDto
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}