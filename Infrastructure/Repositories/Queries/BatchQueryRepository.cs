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
    public class BatchQueryRepository : QueryRepository, IBatchQueryRepository
    {
        public BatchQueryRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<BatchDto?> GetByIdAsync(Guid id)
        {
            var query = "SELECT Id, ProductId, ExpiryDate, ManufactureDate FROM Batches WHERE Id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<BatchDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<BatchDto>> GetAllTableAsync()
        {
            var query = "SELECT Id, ProductId, ExpiryDate, ManufactureDate FROM Batches";
            var result = await _connection.QueryAsync<BatchDto>(query);
            return result;
        }

        public async Task<IEnumerable<BatchDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = @"SELECT b.Id, b.ProductId, b.ExpiryDate, b.ManufactureDate 
                         FROM Batches b
                         INNER JOIN Products p ON b.ProductId = p.Id
                         WHERE p.BrandId = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<BatchDto>(query, parameters);
            return result;
        }
    }
}
