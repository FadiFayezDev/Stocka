using Dapper;
using Inventory.Application.Repositories.Queries;
using Inventory.Contracts.DTOs.Batch;
using Inventory.Infrastructure.Repositories.Queries.Bases;
using Inventory.Primitives.Identifiers;
using BuildingBlocks.Application.Primitives.Identifiers;
using BuildingBlocks.Application.Common.Models;
using System.Data;

namespace Inventory.Infrastructure.Repositories.Queries
{
    internal class BatchQueryRepository : QueryRepository, IBatchQueryRepository
    {
        private const string BaseSelect =
            "SELECT Id, ProductId, PurchaseItemId, BrandId, InitialQuantity, RemainingQuantity, UnitCost, CreatedAt FROM " + TableBatches;

        public BatchQueryRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<IEnumerable<BatchDto>> GetAllAsync(BrandId brandId)
        {
            var sql = $"{BaseSelect} WHERE BrandId = @BrandId ORDER BY CreatedAt DESC";
            return await _connection.QueryAsync<BatchDto>(sql, new { BrandId = brandId.Value });
        }

        public async Task<PaginatedResult<BatchDto>> GetAllPagedAsync(BrandId brandId, int page, int pageSize)
        {
            return await GetPagedCoreAsync(
                brandId,
                " WHERE BrandId = @BrandId",
                new { BrandId = brandId.Value },
                page, pageSize);
        }

        public async Task<PaginatedResult<BatchDto>> GetByProductAsync(BrandId brandId, Guid productId, int page, int pageSize)
        {
            if (productId == Guid.Empty) throw new ArgumentException("Product ID cannot be empty.", nameof(productId));
            return await GetPagedCoreAsync(
                brandId,
                " WHERE BrandId = @BrandId AND ProductId = @ProductId",
                new { BrandId = brandId.Value, ProductId = productId },
                page, pageSize);
        }

        public async Task<PaginatedResult<BatchDto>> GetByPurchaseItemAsync(BrandId brandId, Guid purchaseItemId, int page, int pageSize)
        {
            if (purchaseItemId == Guid.Empty) throw new ArgumentException("Purchase item ID cannot be empty.", nameof(purchaseItemId));
            return await GetPagedCoreAsync(
                brandId,
                " WHERE BrandId = @BrandId AND PurchaseItemId = @PurchaseItemId",
                new { BrandId = brandId.Value, PurchaseItemId = purchaseItemId },
                page, pageSize);
        }

        private async Task<PaginatedResult<BatchDto>> GetPagedCoreAsync(BrandId brandId, string where, object parameters, int page, int pageSize)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var countSql = "SELECT COUNT(*) FROM " + TableBatches + where;
            var total = await _connection.ExecuteScalarAsync<int>(countSql, parameters);

            var offset = (page - 1) * pageSize;
            var sql = $"{BaseSelect}{where} ORDER BY CreatedAt DESC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            var merged = new System.Collections.Generic.Dictionary<string, object?>();
            foreach (var prop in parameters.GetType().GetProperties())
                merged[prop.Name] = prop.GetValue(parameters);
            merged["Offset"] = offset;
            merged["PageSize"] = pageSize;

            var items = await _connection.QueryAsync<BatchDto>(sql, merged);
            return new PaginatedResult<BatchDto>(items.ToList(), total, page, pageSize);
        }

        public async Task<IEnumerable<BatchDto>> GetByWarehouseAsync(BrandId brandId, WarehouseId warehouseId)
        {
            // Batches currently holding (positive) stock in that warehouse.
            var sql = $"""
                SELECT b.Id, b.ProductId, b.PurchaseItemId, b.BrandId, b.InitialQuantity, b.RemainingQuantity, b.UnitCost, b.CreatedAt
                FROM {TableBatches} b
                JOIN {TableWarehouseBatches} wb ON wb.BatchId = b.Id
                WHERE wb.WarehouseId = @WarehouseId AND wb.Quantity > 0 AND wb.BrandId = @BrandId
                ORDER BY b.CreatedAt DESC
                """;
            return await _connection.QueryAsync<BatchDto>(sql, new { WarehouseId = warehouseId.Value, BrandId = brandId.Value });
        }

        public async Task<IEnumerable<BatchDto>> GetUnallocatedAsync(BrandId brandId)
        {
            var sql = $"{BaseSelect} WHERE BrandId = @BrandId AND RemainingQuantity > 0 ORDER BY CreatedAt DESC";
            return await _connection.QueryAsync<BatchDto>(sql, new { BrandId = brandId.Value });
        }

        public async Task<IEnumerable<BatchDto>> GetExhaustedAsync(BrandId brandId)
        {
            var sql = $"{BaseSelect} WHERE BrandId = @BrandId AND RemainingQuantity = 0 ORDER BY CreatedAt DESC";
            return await _connection.QueryAsync<BatchDto>(sql, new { BrandId = brandId.Value });
        }

        public async Task<BatchDto?> GetByIdAsync(BrandId brandId, BatchId id)
        {
            var sql = $"{BaseSelect} WHERE Id = @Id AND BrandId = @BrandId";
            return await _connection.QueryFirstOrDefaultAsync<BatchDto>(sql, new { Id = id.Value, BrandId = brandId.Value });
        }
    }
}
