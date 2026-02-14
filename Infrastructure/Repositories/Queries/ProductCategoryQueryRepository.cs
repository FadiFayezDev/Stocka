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
            var query = "SELECT Id, BrandId, Name FROM ProductCategories WHERE Id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<ProductCategoryDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetAllTableAsync()
        {
            var query = "SELECT Id, BrandId, Name FROM ProductCategories";
            var result = await _connection.QueryAsync<ProductCategoryDto>(query);
            return result;
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = "SELECT Id, BrandId, Name FROM ProductCategories WHERE BrandId = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<ProductCategoryDto>(query, parameters);
            return result;
        }

        public async Task<ProductCategoryIncludedBrandDto?> GetProductCategoryByIdWithBrandsAsync(Guid id)
        {
            var query = @"SELECT pc.Id, pc.BrandId, pc.Name, b.Name AS BrandName 
                         FROM ProductCategories pc
                         LEFT JOIN Brands b ON pc.BrandId = b.Id
                         WHERE pc.Id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<ProductCategoryIncludedBrandDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<ProductCategoryIncludedBrandDto>> GetAllProductCategoriesWithBrandsAsync(Guid brandId)
        {
            var query = @"SELECT pc.Id, pc.BrandId, pc.Name, b.Name AS BrandName 
                         FROM ProductCategories pc
                         LEFT JOIN Brands b ON pc.BrandId = b.Id
                         WHERE pc.BrandId = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<ProductCategoryIncludedBrandDto>(query, parameters);
            return result;
        }
    }
}
