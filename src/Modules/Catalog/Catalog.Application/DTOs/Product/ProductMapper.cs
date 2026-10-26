using Catalog.Domain.Entities;

namespace Catalog.Application.DTOs.Product
{
    public static class ProductMapper
    {
        public static ProductDto ToDto(this Catalog.Domain.Entities.Product product)
            => new(
                product.Id.Value,
                product.BrandId.Value,
                product.Name,
                product.Barcode ?? string.Empty,
                product.CategoryId.Value,
                product.SellingPrice,
                product.IsActive,
                product.ImagePath ?? string.Empty);
    }
}