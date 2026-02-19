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
    public class ProductQueryRepository : QueryRepository, IProductQueryRepository
    {
        public ProductQueryRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<ProductDto?> GetByIdAsync(Guid id)
        {
            var query = $"SELECT id, brand_id AS BrandId, category_id AS CategoryId, name AS Name, barcode AS Barcode FROM {TableProducts} WHERE id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<ProductDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<ProductDto>> GetAllTableAsync()
        {
            var query = $"SELECT id, brand_id AS BrandId, category_id AS CategoryId, name AS Name, barcode AS Barcode FROM {TableProducts}";
            var result = await _connection.QueryAsync<ProductDto>(query);
            return result;
        }

        public async Task<IEnumerable<ProductDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = $"SELECT id, brand_id AS BrandId, category_id AS CategoryId, name AS Name, barcode AS Barcode FROM {TableProducts} WHERE brand_id = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<ProductDto>(query, parameters);
            return result;
        }
    }
}
