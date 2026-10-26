using Catalog.Domain.Entities;

namespace Catalog.Application.DTOs.Category
{
    public static class CategoryMapper
    {
        public static CategoryDto ToDto(this Catalog.Domain.Entities.Category category)
            => new(
                category.Id.Value,
                category.BrandId.Value,
                category.Name,
                category.IsActive);
    }
}