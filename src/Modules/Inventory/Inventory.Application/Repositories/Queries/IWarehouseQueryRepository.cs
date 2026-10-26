using BuildingBlocks.Application.Common.Models;
using BuildingBlocks.Application.Primitives.Identifiers;
using Inventory.Contracts.DTOs.Warehouse;
using Inventory.Primitives.Identifiers;

namespace Inventory.Application.Repositories.Queries
{
    public interface IWarehouseQueryRepository
    {
        Task<WarehouseDto?> GetByIdAsync(BrandId brandId, WarehouseId id);
        Task<IEnumerable<WarehouseDto>> GetAllAsync(BrandId brandId);

        Task<PaginatedResult<WarehouseDto>> GetPagedAsync(BrandId brandId, int page, int pageSize);

        /// <summary>Paged list, optionally filtered to a single activation state.</summary>
        Task<PaginatedResult<WarehouseDto>> GetPagedAsync(BrandId brandId, bool? isActive, int page, int pageSize);

        /// <summary>Warehouses whose name CONTAINS the search value (existing name-matching convention).</summary>
        Task<IEnumerable<WarehouseDto>> GetByNameAsync(BrandId brandId, string name);

        /// <summary>Exact-name match, used to prevent duplicate warehouse names within a Brand.</summary>
        Task<WarehouseDto?> GetByExactNameAsync(BrandId brandId, string name);
    }
}
