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
    public class ProductCategoryQueryRepository : QueryRepository, IProductCategoryQueryRepository
    {
        public ProductCategoryQueryRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<ProductCategoryDto?> GetByIdAsync(Guid id)
        {
            var query = $"SELECT id, brand_id AS BrandId, name AS Name FROM {TableProductCategories} WHERE id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<ProductCategoryDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetAllTableAsync()
        {
            var query = $"SELECT id, brand_id AS BrandId, name AS Name FROM {TableProductCategories}";
            var result = await _connection.QueryAsync<ProductCategoryDto>(query);
            return result;
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = $"SELECT id, brand_id AS BrandId, name AS Name FROM {TableProductCategories} WHERE brand_id = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<ProductCategoryDto>(query, parameters);
            return result;
        }

        public async Task<ProductCategoryIncludedBrandDto?> GetProductCategoryByIdWithBrandsAsync(Guid id)
        {
            var query = $@"SELECT pc.id, pc.brand_id AS BrandId, pc.name AS Name, b.name AS BrandName
                         FROM {TableProductCategories} pc
                         LEFT JOIN {TableBrands} b ON pc.brand_id = b.id
                         WHERE pc.id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<ProductCategoryIncludedBrandDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<ProductCategoryIncludedBrandDto>> GetAllProductCategoriesWithBrandsAsync(Guid brandId)
        {
            var query = $@"SELECT pc.id, pc.brand_id AS BrandId, pc.name AS Name, b.name AS BrandName 
                         FROM {TableProductCategories} pc
                         LEFT JOIN {TableBrands} b ON pc.brand_id = b.id
                         WHERE pc.brand_id = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<ProductCategoryIncludedBrandDto>(query, parameters);
            return result;
        }
    }
}
