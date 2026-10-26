using BuildingBlocks.Application.Primitives.Identifiers;
using Catalog.Application.DTOs.Product;
using Catalog.Application.Repositories.Queries.Bases;
using Catalog.Application.Repositories.Queries.Product;
using Catalog.Infrastructure.Contexts;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Catalog.Infrastructure.Repositories.Queries
{
    internal class ProductQueryRepository : QueryRepository, IProductQueryRepository
    {
        public ProductQueryRepository(IDbConnection connection) : base(connection)
        {

        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync(BrandId brandId)
        {
            var query = "SELECT " +
                        "Id, " +
                        "BrandId, " +
                        "Name, " +
                        "Barcode, " +
                        "CategoryId, " +
                        "SellingPrice AS [Price], " +
                        "IsActive, " +
                        "ImagePath " +
                       $"FROM {TableProducts} " +
                         "WHERE BrandId = @BrandId " +
                         "ORDER BY Name";
            var parameters = new { BrandId = brandId.Value };
            var result = await _connection.QueryAsync<ProductDto>(query, parameters);
            return result;
        }

        public async Task<BuildingBlocks.Application.Common.Models.PaginatedResult<ProductDto>> GetPagedAsync(BrandId brandId, int page, int pageSize)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);
            var countSql = $"SELECT COUNT(*) FROM {TableProducts} WHERE BrandId = @BrandId";
            var total = await _connection.ExecuteScalarAsync<int>(countSql, new { BrandId = brandId.Value });

            var offset = (page - 1) * pageSize;
            var query = "SELECT " +
                        "Id, BrandId, Name, Barcode, CategoryId, SellingPrice AS [Price], IsActive, ImagePath " +
                       $"FROM {TableProducts} WHERE BrandId = @BrandId ORDER BY Name OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            var items = await _connection.QueryAsync<ProductDto>(query, new { BrandId = brandId.Value, Offset = offset, PageSize = pageSize });
            return new BuildingBlocks.Application.Common.Models.PaginatedResult<ProductDto>(items.ToList(), total, page, pageSize);
        }

        public async Task<ProductDto?> GetByIdAsync(Guid id)
        {
            var query = "SELECT " +
                        "Id, " +
                        "BrandId, " +
                        "Name, " +               
                        "Barcode, " +            
                        "CategoryId, " +        
                        "SellingPrice AS [Price], " +
                        "IsActive, " +          
                        "ImagePath " +           
                       $"FROM {TableProducts} " +
                        "WHERE Id = @Id";

            var parameters = new { Id = id };
            return await _connection.QueryFirstOrDefaultAsync<ProductDto>(query, parameters);
        }

        public async Task<ProductDto?> GetByNameAsync(string name)
        {
            var query = "SELECT " +
                        "Id, " +
                        "BrandId, " +
                        "Name, " +
                        "Barcode, " +
                        "CategoryId, " +
                        "SellingPrice AS [Price], " +
                        "IsActive, " +
                        "ImagePath " +
                       $"FROM {TableProducts} " +
                        "WHERE Name = @Name";
            var parameters = new { Name = name };
            var result = await _connection.QueryFirstOrDefaultAsync<ProductDto>(query, parameters);
            return result;
        }
    }
}
