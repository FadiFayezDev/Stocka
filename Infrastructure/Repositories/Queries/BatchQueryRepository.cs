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
            var query = $"SELECT id, product_id AS ProductId, expiry_date AS ExpiryDate, manufacture_date AS ManufactureDate FROM {TableBatches} WHERE id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<BatchDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<BatchDto>> GetAllTableAsync()
        {
            var query = $"SELECT id, product_id AS ProductId, expiry_date AS ExpiryDate, manufacture_date AS ManufactureDate FROM {TableBatches}";
            var result = await _connection.QueryAsync<BatchDto>(query);
            return result;
        }

        public async Task<IEnumerable<BatchDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = $@"SELECT b.id, b.product_id AS ProductId, b.expiry_date AS ExpiryDate, b.manufacture_date AS ManufactureDate 
                         FROM {TableBatches} b
                         INNER JOIN {TableProducts} p ON b.product_id = p.id
                         WHERE p.brand_id = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<BatchDto>(query, parameters);
            return result;
        }
    }
}
