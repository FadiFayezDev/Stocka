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
            var query = "SELECT Id, OrderId, ProductId, BatchId, Quantity, UnitPrice, CostPrice FROM OrderItems WHERE Id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<OrderItemDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<OrderItemDto>> GetAllTableAsync()
        {
            var query = "SELECT Id, OrderId, ProductId, BatchId, Quantity, UnitPrice, CostPrice FROM OrderItems";
            var result = await _connection.QueryAsync<OrderItemDto>(query);
            return result;
        }

        public async Task<IEnumerable<OrderItemDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = @"SELECT si.Id, si.OrderId, si.ProductId, si.BatchId, si.Quantity, si.UnitPrice, si.CostPrice 
                         FROM OrderItems si
                         INNER JOIN Orders s ON si.OrderId = s.Id
                         WHERE s.BrandId = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<OrderItemDto>(query, parameters);
            return result;
        }
    }
}
