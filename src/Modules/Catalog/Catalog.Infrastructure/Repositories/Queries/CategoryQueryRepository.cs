using BuildingBlocks.Application.Primitives.Identifiers;
using Catalog.Application.DTOs.Category;
using Catalog.Application.Repositories.Queries.Bases;
using Catalog.Application.Repositories.Queries.Category;
using Dapper;
using System.Data;

namespace Catalog.Infrastructure.Repositories.Queries
{
    internal class CategoryQueryRepository : QueryRepository, ICategoryQueryRepository
    {
        public CategoryQueryRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync(BrandId brandId)
        {
            var query = "SELECT Id, BrandId, Name, IsActive " +
                       $"FROM {TableCategories} WHERE BrandId = @BrandId ORDER BY Name";
            var parameters = new { BrandId = brandId.Value };
            var result = await _connection.QueryAsync<CategoryDto>(query, parameters);
            return result;
        }

        public async Task<BuildingBlocks.Application.Common.Models.PaginatedResult<CategoryDto>> GetPagedAsync(BrandId brandId, int page, int pageSize)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);
            var countSql = $"SELECT COUNT(*) FROM {TableCategories} WHERE BrandId = @BrandId";
            var total = await _connection.ExecuteScalarAsync<int>(countSql, new { BrandId = brandId.Value });
            var offset = (page - 1) * pageSize;
            var query = $"SELECT Id, BrandId, Name, IsActive FROM {TableCategories} WHERE BrandId = @BrandId ORDER BY Name OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            var items = await _connection.QueryAsync<CategoryDto>(query, new { BrandId = brandId.Value, Offset = offset, PageSize = pageSize });
            return new BuildingBlocks.Application.Common.Models.PaginatedResult<CategoryDto>(items.ToList(), total, page, pageSize);
        }

        public async Task<CategoryDto?> GetByIdAsync(Guid id)
        {
            var query = "SELECT " +
                        "Id, " +
                        "BrandId, " +
                        "Name, " +
                        "IsActive " +
                       $"FROM {TableCategories} " +
                        "WHERE Id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QueryFirstOrDefaultAsync<CategoryDto>(query, parameters);
            return result;
        }

        public async Task<CategoryDto?> GetByNameAsync(string name)
        {
            var query = "SELECT " +
                        "Id, " +
                        "BrandId, " +
                        "Name, " +
                        "IsActive " +
                       $"FROM {TableCategories} " +
                        "WHERE Name = @Name";
            var parameters = new { Name = name };
            var result = await _connection.QueryFirstOrDefaultAsync<CategoryDto>(query, parameters);
            return result;
        }
    }
}
