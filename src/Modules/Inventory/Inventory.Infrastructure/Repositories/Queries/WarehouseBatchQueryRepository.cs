using Dapper;
using Inventory.Application.Repositories.Queries;
using Inventory.Contracts.DTOs.WarehouseBatch;
using Inventory.Infrastructure.Repositories.Queries.Bases;
using Inventory.Primitives.Identifiers;
using BuildingBlocks.Application.Primitives.Identifiers;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Repositories.Queries
{
    internal class WarehouseBatchQueryRepository : QueryRepository, IWarehouseBatchQueryRepository
    {
        private const string BaseSelect =
            "SELECT Id, WarehouseId, BatchId, Quantity, BrandId FROM " + TableWarehouseBatches;

        public WarehouseBatchQueryRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<IEnumerable<WarehouseBatchDto>> GetAllAsync(BrandId brandId, WarehouseId warehouseId)
        {
            var sql = $"{BaseSelect} WHERE WarehouseId = @WarehouseId AND BrandId = @BrandId ORDER BY BatchId";
            return await _connection.QueryAsync<WarehouseBatchDto>(sql, new { WarehouseId = warehouseId.Value, BrandId = brandId.Value });
        }

        public async Task<WarehouseBatchDto?> GetByWarehouseAndBatchAsync(BrandId brandId, WarehouseId warehouseId, BatchId batchId)
        {
            var sql = $"{BaseSelect} WHERE WarehouseId = @WarehouseId AND BatchId = @BatchId AND BrandId = @BrandId";
            return await _connection.QueryFirstOrDefaultAsync<WarehouseBatchDto>(
                sql, new { WarehouseId = warehouseId.Value, BatchId = batchId.Value, BrandId = brandId.Value });
        }

        public async Task<IEnumerable<WarehouseBatchDto>> GetByBatchAsync(BrandId brandId, BatchId batchId)
        {
            var sql = $"{BaseSelect} WHERE BatchId = @BatchId AND BrandId = @BrandId ORDER BY WarehouseId";
            return await _connection.QueryAsync<WarehouseBatchDto>(sql, new { BatchId = batchId.Value, BrandId = brandId.Value });
        }

        public async Task<int> GetQuantityInWarehouseAsync(BrandId brandId, WarehouseId warehouseId, BatchId batchId)
        {
            var sql = $"SELECT COALESCE(SUM(Quantity), 0) FROM {TableWarehouseBatches} WHERE WarehouseId = @WarehouseId AND BatchId = @BatchId AND BrandId = @BrandId";
            return await _connection.ExecuteScalarAsync<int>(
                sql, new { WarehouseId = warehouseId.Value, BatchId = batchId.Value, BrandId = brandId.Value });
        }

        public async Task<int> GetTotalQuantityInWarehousesAsync(BrandId brandId, BatchId batchId)
        {
            var sql = $"SELECT COALESCE(SUM(Quantity), 0) FROM {TableWarehouseBatches} WHERE BatchId = @BatchId AND BrandId = @BrandId";
            return await _connection.ExecuteScalarAsync<int>(sql, new { BatchId = batchId.Value, BrandId = brandId.Value });
        }

        public async Task<WarehouseBatchDto?> GetByIdAsync(BrandId brandId, WarehouseBatchId id)
        {
            var sql = $"{BaseSelect} WHERE Id = @Id AND BrandId = @BrandId";
            return await _connection.QueryFirstOrDefaultAsync<WarehouseBatchDto>(sql, new { Id = id.Value, BrandId = brandId.Value });
        }
    }
}
