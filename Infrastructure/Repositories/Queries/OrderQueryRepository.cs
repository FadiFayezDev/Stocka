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
            var query = $"SELECT id, brand_id AS BrandId, employee_id AS EmployeeId, customer_id AS CustomerId, order_date AS OrderDate, status AS Status, total_amount AS TotalAmount FROM {TableOrders} WHERE id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<OrderDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<OrderDto>> GetAllTableAsync()
        {
            var query = $"SELECT id, brand_id AS BrandId, employee_id AS EmployeeId, customer_id AS CustomerId, order_date AS OrderDate, status AS Status, total_amount AS TotalAmount FROM {TableOrders}";
            var result = await _connection.QueryAsync<OrderDto>(query);
            return result;
        }

        public async Task<IEnumerable<OrderDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = $"SELECT id, brand_id AS BrandId, employee_id AS EmployeeId, customer_id AS CustomerId, order_date AS OrderDate, status AS Status, total_amount AS TotalAmount FROM {TableOrders} WHERE brand_id = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<OrderDto>(query, parameters);
            return result;
        }
    }
}
