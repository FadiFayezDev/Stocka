using Application.Dtos.Sales;
using Application.QueryRepositories;
using Dapper;
using Infrastructure.Repositories.Queries.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Infrastructure.Repositories.Queries
{
    public class SaleQueryRepository : QueryRepository, ISaleQueryRepository
    {
        public SaleQueryRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<SaleDto?> GetByIdAsync(Guid id)
        {
            var query = "SELECT Id, BrandId, EmployeeId, CustomerId, SaleDate, Status, TotalAmount FROM Sales WHERE Id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<SaleDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<SaleDto>> GetAllTableAsync()
        {
            var query = "SELECT Id, BrandId, EmployeeId, CustomerId, SaleDate, Status, TotalAmount FROM Sales";
            var result = await _connection.QueryAsync<SaleDto>(query);
            return result;
        }

        public async Task<IEnumerable<SaleDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = "SELECT Id, BrandId, EmployeeId, CustomerId, SaleDate, Status, TotalAmount FROM Sales WHERE BrandId = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<SaleDto>(query, parameters);
            return result;
        }
    }
}
