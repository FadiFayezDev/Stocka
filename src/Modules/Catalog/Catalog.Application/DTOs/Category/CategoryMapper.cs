using Catalog.Contracts.DTOs.Category;
using Catalog.Domain.Entities;

namespace Catalog.Application.DTOs.Category
{
    /// <summary>
    /// Maps Catalog domain categories onto the public contract DTO (Catalog.Contracts).
    /// </summary>
    public static class CategoryMapper
    {
        public static CategoryDto ToDto(this Catalog.Domain.Entities.Category category)
            => new(
                category.Id.Value,
                category.OrganizationId,
                category.Name,
                category.IsActive);
    }
}
