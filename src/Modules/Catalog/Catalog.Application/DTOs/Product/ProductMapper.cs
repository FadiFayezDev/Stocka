using Catalog.Contracts.DTOs.Product;
using Catalog.Domain.Entities;

namespace Catalog.Application.DTOs.Product
{
    /// <summary>
    /// Maps Catalog domain products onto the public contract DTO (Catalog.Contracts).
    /// </summary>
    public static class ProductMapper
    {
        public static ProductDto ToDto(this Catalog.Domain.Entities.Product product)
            => new(
                product.Id.Value,
                product.OrganizationId,
                product.Name,
                product.Barcode ?? string.Empty,
                product.CategoryId.Value,
                product.SellingPrice,
                product.IsActive,
                product.ImagePath ?? string.Empty);
    }
}
