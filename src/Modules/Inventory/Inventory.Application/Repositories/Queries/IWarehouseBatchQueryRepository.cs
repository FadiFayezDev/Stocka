using BuildingBlocks.Application.Primitives.Identifiers;
using Inventory.Contracts.DTOs.WarehouseBatch;
using Inventory.Primitives.Identifiers;

namespace Inventory.Application.Repositories.Queries
{
    public interface IWarehouseBatchQueryRepository
    {
        Task<WarehouseBatchDto?> GetByIdAsync(BrandId brandId, WarehouseBatchId id);
        Task<IEnumerable<WarehouseBatchDto>> GetAllAsync(BrandId brandId, WarehouseId warehouseId);

        /// <summary>The single warehouse/batch row (unique pair), or null if not yet allocated.</summary>
        Task<WarehouseBatchDto?> GetByWarehouseAndBatchAsync(BrandId brandId, WarehouseId warehouseId, BatchId batchId);

        /// <summary>All warehouse placements of a batch across warehouses.</summary>
        Task<IEnumerable<WarehouseBatchDto>> GetByBatchAsync(BrandId brandId, BatchId batchId);

        /// <summary>Current quantity of a batch in one warehouse (0 when not allocated).</summary>
        Task<int> GetQuantityInWarehouseAsync(BrandId brandId, WarehouseId warehouseId, BatchId batchId);

        /// <summary>Total current units of a batch across all warehouses of the Brand.</summary>
        Task<int> GetTotalQuantityInWarehousesAsync(BrandId brandId, BatchId batchId);
    }
}
