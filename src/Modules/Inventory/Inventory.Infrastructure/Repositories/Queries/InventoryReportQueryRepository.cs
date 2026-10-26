using Dapper;
using Inventory.Application.Repositories.Queries;
using Inventory.Contracts.DTOs.Batch;
using Inventory.Contracts.DTOs.Reports;
using Inventory.Contracts.DTOs.StockMovement;
using Inventory.Infrastructure.Repositories.Queries.Bases;
using BuildingBlocks.Application.Primitives.Identifiers;
using BuildingBlocks.Application.Common.Models;
using System.Collections.Generic;
using System.Data;

namespace Inventory.Infrastructure.Repositories.Queries
{
    /// <summary>
    /// Read-side reporting (Dapper, parameterized, brand-scoped, database-side aggregation).
    /// Movement direction (enum order): PurchaseIn=0, SaleOut=1, TransferIn=2, TransferOut=3,
    /// AdjustmentIn=4, AdjustmentOut=5, OrderReturn=6. Inbound set = {0,2,4,6}, outbound = {1,3,5}.
    /// Reference types: Purchase=0, Order=1, Transfer=2, Adjustment=3, Return=4, ReversalOfMovement=5.
    /// </summary>
    internal class InventoryReportQueryRepository : QueryRepository, IInventoryReportQueryRepository
    {
        public InventoryReportQueryRepository(IDbConnection connection) : base(connection)
        {
        }

        // ---- Warehouse reports ---------------------------------------------------------

        private const string WarehouseStockSummarySql = $"""
            SELECT w.Id AS WarehouseId, w.Name, w.Location, w.IsActive, w.Type,
                   COALESCE(SUM(wb.Quantity), 0) AS TotalUnits,
                   COUNT(DISTINCT b.ProductId) AS DistinctProducts,
                   COALESCE(SUM(wb.Quantity * b.UnitCost), 0) AS TotalCost
            FROM {TableWarehouses} w
            LEFT JOIN {TableWarehouseBatches} wb ON wb.WarehouseId = w.Id AND wb.BrandId = w.BrandId
            LEFT JOIN {TableBatches} b ON b.Id = wb.BatchId AND b.BrandId = wb.BrandId
            WHERE w.BrandId = @BrandId
            GROUP BY w.Id, w.Name, w.Location, w.IsActive, w.Type
            """;
        // HAVING-clause variants (zero-stock / inactive-with-stock) append to this base query.

        public async Task<IEnumerable<WarehouseStockSummaryDto>> GetWarehouseStockSummaryAsync(BrandId brandId)
        {
            var sql = WarehouseStockSummarySql + " ORDER BY w.Name";
            return await _connection.QueryAsync<WarehouseStockSummaryDto>(sql, new { BrandId = brandId.Value });
        }

        public async Task<PaginatedResult<WarehouseStockSummaryDto>> GetWarehousesRankedByStockAsync(BrandId brandId, bool descending, int page, int pageSize)
        {
            var (total, items) = await GetPagedRankedAsync(
                brandId, " ORDER BY TotalUnits " + (descending ? "DESC" : "ASC") + ", w.Name ASC", page, pageSize);
            return new PaginatedResult<WarehouseStockSummaryDto>(items, total, Math.Max(1, page), Math.Clamp(pageSize, 1, 100));
        }

        public async Task<PaginatedResult<WarehouseStockSummaryDto>> GetWarehousesRankedByCostAsync(BrandId brandId, bool descending, int page, int pageSize)
        {
            var (total, items) = await GetPagedRankedAsync(
                brandId, " ORDER BY TotalCost " + (descending ? "DESC" : "ASC") + ", w.Name ASC", page, pageSize);
            return new PaginatedResult<WarehouseStockSummaryDto>(items, total, Math.Max(1, page), Math.Clamp(pageSize, 1, 100));
        }

        private async Task<(int total, List<WarehouseStockSummaryDto> items)> GetPagedRankedAsync(
            BrandId brandId, string orderBy, int page, int pageSize)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var countSql = "SELECT COUNT(*) FROM " + TableWarehouses + " WHERE BrandId = @BrandId";
            var total = await _connection.ExecuteScalarAsync<int>(countSql, new { BrandId = brandId.Value });

            var sql = WarehouseStockSummarySql + orderBy + " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            var items = (await _connection.QueryAsync<WarehouseStockSummaryDto>(
                sql, new { BrandId = brandId.Value, Offset = (page - 1) * pageSize, PageSize = pageSize })).ToList();

            return (total, items);
        }

        public async Task<PaginatedResult<WarehouseInventoryItemDto>> GetWarehouseInventoryAsync(BrandId brandId, Guid warehouseId, int page, int pageSize)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var whereParts = " WHERE wb.WarehouseId = @WarehouseId AND wb.BrandId = @BrandId";
            var countSql = $"""
                SELECT COUNT(*) FROM {TableWarehouseBatches} wb
                JOIN {TableBatches} b ON b.Id = wb.BatchId AND b.BrandId = wb.BrandId
                """ + whereParts;

            var total = await _connection.ExecuteScalarAsync<int>(
                countSql, new { WarehouseId = warehouseId, BrandId = brandId.Value });

            var sql = $"""
                SELECT wb.Id AS WarehouseBatchId, wb.BatchId, b.ProductId, wb.Quantity,
                       b.UnitCost AS BatchUnitCost, (wb.Quantity * b.UnitCost) AS LineCost,
                       b.CreatedAt AS BatchCreatedAt
                FROM {TableWarehouseBatches} wb
                JOIN {TableBatches} b ON b.Id = wb.BatchId AND b.BrandId = wb.BrandId
                """ + whereParts +
                " ORDER BY b.ProductId, wb.BatchId OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            var items = await _connection.QueryAsync<WarehouseInventoryItemDto>(
                sql, new { WarehouseId = warehouseId, BrandId = brandId.Value, Offset = (page - 1) * pageSize, PageSize = pageSize });

            return new PaginatedResult<WarehouseInventoryItemDto>(items.ToList(), total, page, pageSize);
        }

        public async Task<IEnumerable<WarehouseStockSummaryDto>> GetZeroStockWarehousesAsync(BrandId brandId)        {
            var sql = $"{WarehouseStockSummarySql} HAVING COALESCE(SUM(wb.Quantity), 0) = 0 ORDER BY w.Name";
            return await _connection.QueryAsync<WarehouseStockSummaryDto>(sql, new { BrandId = brandId.Value });
        }

        public async Task<IEnumerable<WarehouseStockSummaryDto>> GetInactiveWarehousesWithStockAsync(BrandId brandId)
        {
            var sql = $"{WarehouseStockSummarySql} HAVING COALESCE(SUM(wb.Quantity), 0) > 0 AND w.IsActive = 0 ORDER BY w.Name";
            return await _connection.QueryAsync<WarehouseStockSummaryDto>(sql, new { BrandId = brandId.Value });
        }

        // ---- Stock quantity reports ------------------------------------------------------

        public async Task<IEnumerable<ProductStockRowDto>> GetProductStockDistributionAsync(BrandId brandId, Guid productId)
        {
            var sql = $"""
                SELECT wb.WarehouseId, COALESCE(SUM(wb.Quantity), 0) AS Quantity
                FROM {TableWarehouseBatches} wb
                JOIN {TableBatches} b ON b.Id = wb.BatchId AND b.BrandId = wb.BrandId
                WHERE b.ProductId = @ProductId AND wb.BrandId = @BrandId AND wb.Quantity > 0
                GROUP BY wb.WarehouseId
                ORDER BY Quantity DESC
                """;
            return await _connection.QueryAsync<ProductStockRowDto>(sql, new { ProductId = productId, BrandId = brandId.Value });
        }

        public async Task<int> GetProductTotalUnitsAsync(BrandId brandId, Guid productId)
        {
            var sql = $"""
                SELECT COALESCE(SUM(wb.Quantity), 0)
                FROM {TableWarehouseBatches} wb
                JOIN {TableBatches} b ON b.Id = wb.BatchId AND b.BrandId = wb.BrandId
                WHERE b.ProductId = @ProductId AND wb.BrandId = @BrandId
                """;
            return await _connection.ExecuteScalarAsync<int>(sql, new { ProductId = productId, BrandId = brandId.Value });
        }

        public async Task<int> GetProductUnallocatedUnitsAsync(BrandId brandId, Guid productId)
        {
            var sql = $"SELECT COALESCE(SUM(RemainingQuantity), 0) FROM {TableBatches} WHERE ProductId = @ProductId AND BrandId = @BrandId";
            return await _connection.ExecuteScalarAsync<int>(sql, new { ProductId = productId, BrandId = brandId.Value });
        }

        public async Task<PaginatedResult<ZeroStockProductDto>> GetZeroStockProductsAsync(BrandId brandId, int page, int pageSize)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            // Products with known batches but zero/no stock left in every warehouse
            // (distinct from a product simply not present in a warehouse yet).
            const string baseQuery = $"""
                SELECT b.ProductId,
                       SUM(b.InitialQuantity) AS InitialQuantity,
                       SUM(b.RemainingQuantity) AS UnallocatedQuantity,
                       SUM(COALESCE(wt.Quantity, 0)) AS QuantityInWarehouses
                FROM {TableBatches} b
                LEFT JOIN (SELECT BatchId, SUM(Quantity) AS Quantity FROM {TableWarehouseBatches} WHERE BrandId = @BrandId GROUP BY BatchId) wt
                       ON wt.BatchId = b.Id
                WHERE b.BrandId = @BrandId
                GROUP BY b.ProductId
                HAVING SUM(COALESCE(wt.Quantity, 0)) = 0
                """;

            var total = await _connection.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM ({baseQuery}) z", new { BrandId = brandId.Value });

            var sql = baseQuery + " ORDER BY b.ProductId OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            var items = await _connection.QueryAsync<ZeroStockProductDto>(
                sql, new { BrandId = brandId.Value, Offset = (page - 1) * pageSize, PageSize = pageSize });

            return new PaginatedResult<ZeroStockProductDto>(items.ToList(), total, page, pageSize);
        }

        public async Task<int> GetZeroStockWarehouseBatchRowsCountAsync(BrandId brandId)
        {
            var sql = $"SELECT COUNT(*) FROM {TableWarehouseBatches} WHERE BrandId = @BrandId AND Quantity = 0";
            return await _connection.ExecuteScalarAsync<int>(sql, new { BrandId = brandId.Value });
        }

        // ---- Valuation (recorded batch unit costs only) ----------------------------------

        // See GetWarehousesRankedByCostAsync above â€” reuse of summary data keeps the rules single-sourced.

        public async Task<decimal> GetTotalInventoryCostAsync(BrandId brandId)
        {
            var sql = $"""
                SELECT COALESCE(SUM(wb.Quantity * b.UnitCost), 0)
                FROM {TableWarehouseBatches} wb
                JOIN {TableBatches} b ON b.Id = wb.BatchId AND b.BrandId = wb.BrandId
                WHERE wb.BrandId = @BrandId
                """;
            return await _connection.ExecuteScalarAsync<decimal>(sql, new { BrandId = brandId.Value });
        }

        // ---- Movement reports -------------------------------------------------------------

        public async Task<IEnumerable<MovementTypeTotalsDto>> GetMovementTotalsByTypeAsync(BrandId brandId, DateTime? dateFromUtc, DateTime? dateToUtc)
        {
            var dateFilter = string.Empty;
            if (dateFromUtc.HasValue) dateFilter += " AND CreatedAt >= @DateFromUtc";
            if (dateToUtc.HasValue) dateFilter += " AND CreatedAt <= @DateToUtc";

            var sql = $"""
                SELECT MovementType,
                       SUM(Quantity) AS TotalQuantity,
                       SUM(CASE WHEN MovementType IN (0, 2, 4, 6) THEN CAST(Quantity AS decimal(19,2)) ELSE -CAST(Quantity AS decimal(19,2)) END) AS SignedQuantity,
                       COUNT(*) AS MovementCount
                FROM {TableStockMovements}
                WHERE BrandId = @BrandId{dateFilter}
                GROUP BY MovementType
                ORDER BY MovementType
                """;

            var parameters = new { BrandId = brandId.Value, dateFromUtc, dateToUtc };
            return await _connection.QueryAsync<MovementTypeTotalsDto>(sql, parameters);
        }

        public async Task<InboundOutboundSummaryDto> GetInboundOutboundTotalsAsync(BrandId brandId, DateTime dateFromUtc, DateTime dateToUtc)
        {
            // Transfer legs (2, 3) are EXCLUDED from both totals to avoid double-counting paired movements.
            var sql = $"""
                SELECT
                    COALESCE(SUM(CASE WHEN MovementType IN (0, 4, 6) THEN Quantity ELSE 0 END), 0) AS InboundUnits,
                    COALESCE(SUM(CASE WHEN MovementType IN (1, 5) THEN Quantity ELSE 0 END), 0) AS OutboundUnits
                FROM {TableStockMovements}
                WHERE BrandId = @BrandId AND CreatedAt >= @DateFromUtc AND CreatedAt <= @DateToUtc
                """;
            var row = await _connection.QuerySingleAsync<(int InboundUnits, int OutboundUnits)>(
                sql, new { BrandId = brandId.Value, DateFromUtc = dateFromUtc, DateToUtc = dateToUtc });

            return new InboundOutboundSummaryDto(dateFromUtc, dateToUtc, row.InboundUnits, row.OutboundUnits);
        }

        public async Task<PaginatedResult<TransferHistoryDto>> GetTransferHistoryAsync(BrandId brandId, int page, int pageSize, DateTime? dateFromUtc = null, DateTime? dateToUtc = null)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var dateFilter = string.Empty;
            if (dateFromUtc.HasValue) dateFilter += " AND s.CreatedAt >= @DateFromUtc";
            if (dateToUtc.HasValue) dateFilter += " AND s.CreatedAt <= @DateToUtc";

            // One row per transfer operation: the TransferOut/TransferIn pair sharing one
            // Transfer reference is aggregated â€” never counted twice.
            var baseQuery = $"""
                SELECT s.ReferenceId AS TransferReferenceId,
                       MIN(s.CreatedAt) AS OccurredAtUtc,
                       MIN(s.ProductId) AS ProductId,
                       MIN(s.BatchId) AS BatchId,
                       MAX(s.Quantity) AS Quantity,
                       MAX(CASE WHEN s.MovementType = 3 THEN s.WarehouseId END) AS SourceWarehouseId,
                       MAX(CASE WHEN s.MovementType = 2 THEN s.WarehouseId END) AS DestinationWarehouseId
                FROM {TableStockMovements} s
                WHERE s.BrandId = @BrandId AND s.MovementType IN (2, 3) AND s.ReferenceType = 2 AND s.ReferenceId IS NOT NULL{dateFilter}
                GROUP BY s.ReferenceId
                """;

            var transferParams = new { BrandId = brandId.Value, dateFromUtc, dateToUtc };
            var countSql = $"SELECT COUNT(*) FROM ({baseQuery}) t";
            var total = await _connection.ExecuteScalarAsync<int>(countSql, transferParams);

            var sql = baseQuery + " ORDER BY MIN(s.CreatedAt) DESC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            var items = await _connection.QueryAsync<TransferHistoryDto>(
                sql, new { BrandId = brandId.Value, dateFromUtc, dateToUtc, Offset = (page - 1) * pageSize, PageSize = pageSize });

            return new PaginatedResult<TransferHistoryDto>(items.ToList(), total, page, pageSize);
        }

        public async Task<PaginatedResult<StockMovementDto>> GetAdjustmentHistoryAsync(BrandId brandId, int page, int pageSize, DateTime? dateFromUtc = null, DateTime? dateToUtc = null)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            // Adjustment legs (4, 5) cover physical-count corrections, damage write-offs
            // and reversals (ReferenceType = 5 marks reversal corrections). Reasons preserved.
            var dateFilter = string.Empty;
            if (dateFromUtc.HasValue) dateFilter += " AND s.CreatedAt >= @DateFromUtc";
            if (dateToUtc.HasValue) dateFilter += " AND s.CreatedAt <= @DateToUtc";

            var countSql = $"SELECT COUNT(*) FROM {TableStockMovements} s WHERE s.BrandId = @BrandId AND s.MovementType IN (4, 5){dateFilter}";
            var total = await _connection.ExecuteScalarAsync<int>(countSql, new { BrandId = brandId.Value, dateFromUtc, dateToUtc });

            var sql = $"SELECT Id, ProductId, BatchId, WarehouseId, BrandId, Quantity, MovementType, ReferenceType, ReferenceId, CreatedAt, Reason FROM {TableStockMovements} s WHERE s.BrandId = @BrandId AND s.MovementType IN (4, 5){dateFilter} ORDER BY s.CreatedAt DESC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            var items = await _connection.QueryAsync<StockMovementDto>(
                sql, new { BrandId = brandId.Value, dateFromUtc, dateToUtc, Offset = (page - 1) * pageSize, PageSize = pageSize });

            return new PaginatedResult<StockMovementDto>(items.ToList(), total, page, pageSize);
        }

        // ---- Control / audit ----------------------------------------------------------------

        public async Task<PaginatedResult<HistoricalBalanceDto>> GetHistoricalBalanceAsync(BrandId brandId, DateTime asOfUtc, int page, int pageSize)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            // Balance reconstruction from the audit trail. Transfer legs cancel symmetrically
            // (out leg at source, in leg at destination), so no double counting occurs.
            const string baseQuery = $"""
                SELECT s.ProductId, s.WarehouseId,
                       SUM(CASE WHEN s.MovementType IN (0, 2, 4, 6) THEN s.Quantity ELSE -s.Quantity END) AS Quantity,
                       COUNT(*) AS MovementsConsidered
                FROM {TableStockMovements} s
                WHERE s.BrandId = @BrandId AND s.CreatedAt <= @AsOfUtc
                GROUP BY s.ProductId, s.WarehouseId
                HAVING SUM(CASE WHEN s.MovementType IN (0, 2, 4, 6) THEN s.Quantity ELSE -s.Quantity END) <> 0
                """;

            var total = await _connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM (" + baseQuery + ") h", new { BrandId = brandId.Value, AsOfUtc = asOfUtc });

            var sql = baseQuery + " ORDER BY s.ProductId, s.WarehouseId OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            var items = await _connection.QueryAsync<HistoricalBalanceDto>(
                sql, new { BrandId = brandId.Value, AsOfUtc = asOfUtc, Offset = (page - 1) * pageSize, PageSize = pageSize });

            return new PaginatedResult<HistoricalBalanceDto>(items.ToList(), total, page, pageSize);
        }

        public async Task<IEnumerable<BatchDto>> GetBatchesWithZeroRemainingAsync(BrandId brandId)
        {
            var sql = $"SELECT Id, ProductId, PurchaseItemId, BrandId, InitialQuantity, RemainingQuantity, UnitCost, CreatedAt FROM {TableBatches} WHERE BrandId = @BrandId AND RemainingQuantity = 0 ORDER BY CreatedAt DESC";
            return await _connection.QueryAsync<BatchDto>(sql, new { BrandId = brandId.Value });
        }

        // ---- Scalar aggregates ---------------------------------------------------------------

        public async Task<int> GetWarehouseCountAsync(BrandId brandId)
        {
            var sql = $"SELECT COUNT(*) FROM {TableWarehouses} WHERE BrandId = @BrandId";
            return await _connection.ExecuteScalarAsync<int>(sql, new { BrandId = brandId.Value });
        }

        public async Task<int> GetActiveWarehouseCountAsync(BrandId brandId)
        {
            var sql = $"SELECT COUNT(*) FROM {TableWarehouses} WHERE BrandId = @BrandId AND IsActive = 1";
            return await _connection.ExecuteScalarAsync<int>(sql, new { BrandId = brandId.Value });
        }

        public async Task<int> GetBatchCountAsync(BrandId brandId)
        {
            var sql = $"SELECT COUNT(*) FROM {TableBatches} WHERE BrandId = @BrandId";
            return await _connection.ExecuteScalarAsync<int>(sql, new { BrandId = brandId.Value });
        }

        public async Task<int> GetExhaustedBatchCountAsync(BrandId brandId)
        {
            var sql = $"SELECT COUNT(*) FROM {TableBatches} WHERE BrandId = @BrandId AND RemainingQuantity = 0";
            return await _connection.ExecuteScalarAsync<int>(sql, new { BrandId = brandId.Value });
        }

        public async Task<int> GetTotalUnitsInWarehousesAsync(BrandId brandId)
        {
            var sql = $"SELECT COALESCE(SUM(Quantity), 0) FROM {TableWarehouseBatches} WHERE BrandId = @BrandId";
            return await _connection.ExecuteScalarAsync<int>(sql, new { BrandId = brandId.Value });
        }

        public async Task<int> GetTotalUnallocatedUnitsAsync(BrandId brandId)
        {
            var sql = $"SELECT COALESCE(SUM(RemainingQuantity), 0) FROM {TableBatches} WHERE BrandId = @BrandId";
            return await _connection.ExecuteScalarAsync<int>(sql, new { BrandId = brandId.Value });
        }
    }
}
