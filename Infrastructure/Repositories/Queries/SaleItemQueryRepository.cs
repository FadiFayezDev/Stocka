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
    public class SaleItemQueryRepository : QueryRepository, ISaleItemQueryRepository
    {
        public SaleItemQueryRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<SaleItemDto?> GetByIdAsync(Guid id)
        {
            var query = "SELECT Id, SaleId, ProductId, BatchId, Quantity, UnitPrice, CostPrice FROM SaleItems WHERE Id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<SaleItemDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<SaleItemDto>> GetAllTableAsync()
        {
            var query = "SELECT Id, SaleId, ProductId, BatchId, Quantity, UnitPrice, CostPrice FROM SaleItems";
            var result = await _connection.QueryAsync<SaleItemDto>(query);
            return result;
        }

        public async Task<IEnumerable<SaleItemDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = @"SELECT si.Id, si.SaleId, si.ProductId, si.BatchId, si.Quantity, si.UnitPrice, si.CostPrice 
                         FROM SaleItems si
                         INNER JOIN Sales s ON si.SaleId = s.Id
                         WHERE s.BrandId = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<SaleItemDto>(query, parameters);
            return result;
        }
    }
}
