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
            var query = $"SELECT id, product_id AS ProductId, warehouse_id AS WarehouseId, batch_id AS BatchId, movement_type AS MovementType, reference_type AS ReferenceType, quantity AS Quantity, created_at AS MovementDate FROM {TableStockMovements} WHERE id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<StockMovementDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<StockMovementDto>> GetAllTableAsync()
        {
            var query = $"SELECT id, product_id AS ProductId, warehouse_id AS WarehouseId, batch_id AS BatchId, movement_type AS MovementType, reference_type AS ReferenceType, quantity AS Quantity, created_at AS MovementDate FROM {TableStockMovements}";
            var result = await _connection.QueryAsync<StockMovementDto>(query);
            return result;
        }

        public async Task<IEnumerable<StockMovementDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = $@"SELECT sm.id, sm.product_id AS ProductId, sm.warehouse_id AS WarehouseId, sm.batch_id AS BatchId, sm.movement_type AS MovementType, sm.reference_type AS ReferenceType, sm.quantity AS Quantity, sm.created_at AS MovementDate 
                         FROM {TableStockMovements} sm
                         INNER JOIN {TableProducts} p ON sm.product_id = p.id
                         WHERE p.brand_id = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<StockMovementDto>(query, parameters);
            return result;
        }
    }
}
