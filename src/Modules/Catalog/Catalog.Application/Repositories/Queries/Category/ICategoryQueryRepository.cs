using BuildingBlocks.Application.Common.Models;
using BuildingBlocks.Application.Primitives.Identifiers;
using Catalog.Application.DTOs.Category;

namespace Catalog.Application.Repositories.Queries.Category
{
    public interface ICategoryQueryRepository
    {
        Task<CategoryDto?> GetByIdAsync(Guid id);
        Task<CategoryDto?> GetByNameAsync(string name);
        Task<IEnumerable<CategoryDto>> GetAllAsync(BrandId brandId);
        Task<PaginatedResult<CategoryDto>> GetPagedAsync(BrandId brandId, int page, int pageSize);
    }
}