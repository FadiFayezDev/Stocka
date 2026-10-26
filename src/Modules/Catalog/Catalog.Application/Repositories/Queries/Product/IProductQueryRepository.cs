using BuildingBlocks.Application.Common.Models;
using BuildingBlocks.Application.Primitives.Identifiers;
using Catalog.Application.DTOs.Product;

namespace Catalog.Application.Repositories.Queries.Product
{
    public interface IProductQueryRepository
    {
        Task<ProductDto?> GetByIdAsync(Guid id);
        Task<ProductDto?> GetByNameAsync(string name);
        Task<IEnumerable<ProductDto>> GetAllAsync(BrandId brandId);
        Task<PaginatedResult<ProductDto>> GetPagedAsync(BrandId brandId, int page, int pageSize);
    }
}