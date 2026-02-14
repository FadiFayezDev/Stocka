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
            var query = "SELECT Id, WarehouseId, BatchId, Quantity FROM WarehouseBatches WHERE Id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<WarehouseBatchDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<WarehouseBatchDto>> GetAllTableAsync()
        {
            var query = "SELECT Id, WarehouseId, BatchId, Quantity FROM WarehouseBatches";
            var result = await _connection.QueryAsync<WarehouseBatchDto>(query);
            return result;
        }

        public async Task<IEnumerable<WarehouseBatchDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = @"SELECT wb.Id, wb.WarehouseId, wb.BatchId, wb.Quantity 
                         FROM WarehouseBatches wb
                         INNER JOIN Warehouses w ON wb.WarehouseId = w.Id
                         INNER JOIN Branches b ON w.BranchId = b.Id
                         WHERE b.BrandId = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<WarehouseBatchDto>(query, parameters);
            return result;
        }
    }
}
