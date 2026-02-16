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
    public class OrderQueryRepository : QueryRepository, IOrderQueryRepository
    {
        public OrderQueryRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<OrderDto?> GetByIdAsync(Guid id)
        {
            var query = "SELECT Id, BrandId, EmployeeId, CustomerId, OrderDate, Status, TotalAmount FROM Orders WHERE Id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<OrderDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<OrderDto>> GetAllTableAsync()
        {
            var query = "SELECT Id, BrandId, EmployeeId, CustomerId, OrderDate, Status, TotalAmount FROM Orders";
            var result = await _connection.QueryAsync<OrderDto>(query);
            return result;
        }

        public async Task<IEnumerable<OrderDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = "SELECT Id, BrandId, EmployeeId, CustomerId, OrderDate, Status, TotalAmount FROM Orders WHERE BrandId = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<OrderDto>(query, parameters);
            return result;
        }
    }
}
