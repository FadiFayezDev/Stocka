using Application.Dtos.Orders;
using Application.QueryRepositories;
using Dapper;
using Infrastructure.Repositories.Queries.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Infrastructure.Repositories.Queries
{
    public class OrderItemQueryRepository : QueryRepository, IOrderItemQueryRepository
    {
        public OrderItemQueryRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<OrderItemDto?> GetByIdAsync(Guid id)
        {
            var query = $"SELECT id, order_id AS OrderId, product_id AS ProductId, batch_id AS BatchId, quantity AS Quantity, unit_price AS UnitPrice, cost_price AS CostPrice FROM {TableOrderItems} WHERE id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<OrderItemDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<OrderItemDto>> GetAllTableAsync()
        {
            var query = $"SELECT id, order_id AS OrderId, product_id AS ProductId, batch_id AS BatchId, quantity AS Quantity, unit_price AS UnitPrice, cost_price AS CostPrice FROM {TableOrderItems}";
            var result = await _connection.QueryAsync<OrderItemDto>(query);
            return result;
        }

        public async Task<IEnumerable<OrderItemDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = $@"SELECT si.id, si.order_id AS OrderId, si.product_id AS ProductId, si.batch_id AS BatchId, si.quantity AS Quantity, si.unit_price AS UnitPrice, si.cost_price AS CostPrice 
                         FROM {TableOrderItems} si
                         INNER JOIN {TableOrders} s ON si.order_id = s.id
                         WHERE s.brand_id = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<OrderItemDto>(query, parameters);
            return result;
        }
    }
}
