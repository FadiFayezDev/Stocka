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
    public class StockMovementQueryRepository : QueryRepository, IStockMovementQueryRepository
    {
        public StockMovementQueryRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<StockMovementDto?> GetByIdAsync(Guid id)
        {
            var query = "SELECT Id, ProductId, WarehouseId, BatchId, MovementType, ReferenceType, Quantity, MovementDate FROM StockMovements WHERE Id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<StockMovementDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<StockMovementDto>> GetAllTableAsync()
        {
            var query = "SELECT Id, ProductId, WarehouseId, BatchId, MovementType, ReferenceType, Quantity, MovementDate FROM StockMovements";
            var result = await _connection.QueryAsync<StockMovementDto>(query);
            return result;
        }

        public async Task<IEnumerable<StockMovementDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = @"SELECT sm.Id, sm.ProductId, sm.WarehouseId, sm.BatchId, sm.MovementType, sm.ReferenceType, sm.Quantity, sm.MovementDate 
                         FROM StockMovements sm
                         INNER JOIN Products p ON sm.ProductId = p.Id
                         WHERE p.BrandId = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<StockMovementDto>(query, parameters);
            return result;
        }
    }
}
