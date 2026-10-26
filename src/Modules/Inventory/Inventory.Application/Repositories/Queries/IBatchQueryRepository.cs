using BuildingBlocks.Application.Common.Models;
using BuildingBlocks.Application.Primitives.Identifiers;
using Inventory.Contracts.DTOs.Batch;
using Inventory.Primitives.Identifiers;

namespace Inventory.Application.Repositories.Queries
{
    public interface IBatchQueryRepository
    {
        Task<BatchDto?> GetByIdAsync(BrandId brandId, BatchId id);
        Task<IEnumerable<BatchDto>> GetAllAsync(BrandId brandId);

        Task<PaginatedResult<BatchDto>> GetAllPagedAsync(BrandId brandId, int page, int pageSize);

        /// <summary>Batches of one product (by the batch's soft Catalog product reference).</summary>
        Task<PaginatedResult<BatchDto>> GetByProductAsync(BrandId brandId, Guid productId, int page, int pageSize);

        /// <summary>Batches originating from one purchase item.</summary>
        Task<PaginatedResult<BatchDto>> GetByPurchaseItemAsync(BrandId brandId, Guid purchaseItemId, int page, int pageSize);

        /// <summary>Batches currently holding stock (Quantity &gt; 0) in the given warehouse.</summary>
        Task<IEnumerable<BatchDto>> GetByWarehouseAsync(BrandId brandId, WarehouseId warehouseId);

        /// <summary>Batches with units not yet allocated to any warehouse (RemainingQuantity &gt; 0).</summary>
        Task<IEnumerable<BatchDto>> GetUnallocatedAsync(BrandId brandId);

        /// <summary>Batches with no allocatable units left (RemainingQuantity = 0).</summary>
        Task<IEnumerable<BatchDto>> GetExhaustedAsync(BrandId brandId);
    }
}
