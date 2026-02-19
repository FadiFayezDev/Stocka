using Application.Dtos.Products;
using Application.QueryRepositories;
using Dapper;
using Infrastructure.Repositories.Queries.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Infrastructure.Repositories.Queries
{
    public class WarehouseBatchQueryRepository : QueryRepository, IWarehouseBatchQueryRepository
    {
        public WarehouseBatchQueryRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<WarehouseBatchDto?> GetByIdAsync(Guid id)
        {
            var query = $"SELECT id, warehouse_id AS WarehouseId, batch_id AS BatchId, quantity AS Quantity FROM {TableWarehouseBatches} WHERE id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<WarehouseBatchDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<WarehouseBatchDto>> GetAllTableAsync()
        {
            var query = $"SELECT id, warehouse_id AS WarehouseId, batch_id AS BatchId, quantity AS Quantity FROM {TableWarehouseBatches}";
            var result = await _connection.QueryAsync<WarehouseBatchDto>(query);
            return result;
        }

        public async Task<IEnumerable<WarehouseBatchDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = $@"SELECT wb.id, wb.warehouse_id AS WarehouseId, wb.batch_id AS BatchId, wb.quantity AS Quantity 
                         FROM {TableWarehouseBatches} wb
                         INNER JOIN Warehouses w ON wb.warehouse_id = w.id
                         INNER JOIN {TableBranches} b ON w.branch_id = b.id
                         WHERE b.brand_id = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<WarehouseBatchDto>(query, parameters);
            return result;
        }
    }
}
