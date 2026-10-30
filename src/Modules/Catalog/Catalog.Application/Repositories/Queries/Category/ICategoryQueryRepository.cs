using BuildingBlocks.Application.Common.Models;
using Catalog.Contracts.DTOs.Category;

namespace Catalog.Application.Repositories.Queries.Category
{
    public interface ICategoryQueryRepository
    {
        Task<CategoryDto?> GetByIdAsync(Guid id);
        Task<CategoryDto?> GetByNameAsync(string name);
        Task<IEnumerable<CategoryDto>> GetAllAsync(Guid organizationId);
        Task<PaginatedResult<CategoryDto>> GetPagedAsync(Guid organizationId, int page, int pageSize);
    }
}