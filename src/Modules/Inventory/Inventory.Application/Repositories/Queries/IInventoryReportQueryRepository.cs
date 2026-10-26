using BuildingBlocks.Application.Common.Models;
using BuildingBlocks.Application.Primitives.Identifiers;
using Inventory.Contracts.DTOs.Batch;
using Inventory.Contracts.DTOs.Reports;
using Inventory.Contracts.DTOs.StockMovement;

namespace Inventory.Application.Repositories.Queries
{
    /// <summary>
    /// Read-side reporting repository (Dapper). All queries are brand-scoped, parameterized,
    /// database-aggregated and return untracked DTO projections. Never a write source of truth:
    /// current quantities always come from Inventory.WarehouseBatches / Inventory.Batches.
    /// </summary>
    public interface IInventoryReportQueryRepository
    {
        // ---- Warehouse reports ----
        Task<IEnumerable<WarehouseStockSummaryDto>> GetWarehouseStockSummaryAsync(BrandId brandId);
        Task<PaginatedResult<WarehouseStockSummaryDto>> GetWarehousesRankedByStockAsync(BrandId brandId, bool descending, int page, int pageSize);
        Task<PaginatedResult<WarehouseInventoryItemDto>> GetWarehouseInventoryAsync(BrandId brandId, Guid warehouseId, int page, int pageSize);
        Task<IEnumerable<WarehouseStockSummaryDto>> GetZeroStockWarehousesAsync(BrandId brandId);
        Task<IEnumerable<WarehouseStockSummaryDto>> GetInactiveWarehousesWithStockAsync(BrandId brandId);

        // ---- Stock quantity reports ----
        Task<IEnumerable<ProductStockRowDto>> GetProductStockDistributionAsync(BrandId brandId, Guid productId);
        Task<int> GetProductTotalUnitsAsync(BrandId brandId, Guid productId);
        Task<int> GetProductUnallocatedUnitsAsync(BrandId brandId, Guid productId);
        Task<PaginatedResult<ZeroStockProductDto>> GetZeroStockProductsAsync(BrandId brandId, int page, int pageSize);
        /// <summary>Warehouse/batch rows that still hold a zero quantity (batch fully consumed in that warehouse).</summary>
        Task<int> GetZeroStockWarehouseBatchRowsCountAsync(BrandId brandId);

        // ---- Valuation (recorded batch unit costs only â€” no valuation policy approved) ----
        Task<PaginatedResult<WarehouseStockSummaryDto>> GetWarehousesRankedByCostAsync(BrandId brandId, bool descending, int page, int pageSize);
        Task<decimal> GetTotalInventoryCostAsync(BrandId brandId);

        // ---- Movement reports ----
        Task<IEnumerable<MovementTypeTotalsDto>> GetMovementTotalsByTypeAsync(BrandId brandId, DateTime? dateFromUtc, DateTime? dateToUtc);
        Task<InboundOutboundSummaryDto> GetInboundOutboundTotalsAsync(BrandId brandId, DateTime dateFromUtc, DateTime dateToUtc);
        /// <summary>Transfer operations, one row per transfer (TransferOut/TransferIn pair aggregated â€” no double counting).</summary>
        Task<PaginatedResult<TransferHistoryDto>> GetTransferHistoryAsync(BrandId brandId, int page, int pageSize, DateTime? dateFromUtc = null, DateTime? dateToUtc = null);
        /// <summary>Adjustment-type movements (physical counts, damage write-offs, reversals) with recorded reasons.</summary>
        Task<PaginatedResult<StockMovementDto>> GetAdjustmentHistoryAsync(BrandId brandId, int page, int pageSize, DateTime? dateFromUtc = null, DateTime? dateToUtc = null);

        // ---- Control / audit ----
        Task<PaginatedResult<HistoricalBalanceDto>> GetHistoricalBalanceAsync(BrandId brandId, DateTime asOfUtc, int page, int pageSize);
        Task<IEnumerable<BatchDto>> GetBatchesWithZeroRemainingAsync(BrandId brandId);

        // ---- Scalar aggregates for summaries ----
        Task<int> GetWarehouseCountAsync(BrandId brandId);
        Task<int> GetActiveWarehouseCountAsync(BrandId brandId);
        Task<int> GetBatchCountAsync(BrandId brandId);
        Task<int> GetExhaustedBatchCountAsync(BrandId brandId);
        Task<int> GetTotalUnitsInWarehousesAsync(BrandId brandId);
        Task<int> GetTotalUnallocatedUnitsAsync(BrandId brandId);
    }
}
