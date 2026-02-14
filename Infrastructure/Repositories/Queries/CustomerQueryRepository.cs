using Application.Dtos.Core;
using Application.QueryRepositories;
using Dapper;
using Infrastructure.Repositories.Queries.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Infrastructure.Repositories.Queries
{
    public class CustomerQueryRepository : QueryRepository, ICustomerQueryRepository
    {
        public CustomerQueryRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<CustomerDto?> GetByIdAsync(Guid id)
        {
            var query = "SELECT Id, UserId, BrandId, LoyaltyPoints FROM Customers WHERE Id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<CustomerDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllTableAsync()
        {
            var query = "SELECT Id, UserId, BrandId, LoyaltyPoints FROM Customers";
            var result = await _connection.QueryAsync<CustomerDto>(query);
            return result;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = "SELECT Id, UserId, BrandId, LoyaltyPoints FROM Customers WHERE BrandId = @BrandId OR BrandId IS NULL";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<CustomerDto>(query, parameters);
            return result;
        }
    }
}
