using Catalog.Contracts.DTOs.Category;
using Catalog.Application.Common.Interfaces;
using Catalog.Application.Repositories.Queries.Bases;
using Catalog.Application.Repositories.Queries.Category;
using Dapper;

namespace Catalog.Infrastructure.Repositories.Queries
{
    internal class CategoryQueryRepository : QueryRepository, ICategoryQueryRepository
    {
        public CategoryQueryRepository(ICatalogDbConnection connection) : base(connection)
        {
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync(Guid organizationId)
        {
            var query = "SELECT Id, OrganizationId, Name, IsActive " +
                       $"FROM {TableCategories} WHERE OrganizationId = @OrganizationId ORDER BY Name";
            var parameters = new { OrganizationId = organizationId };
            var result = await _connection.QueryAsync<CategoryDto>(query, parameters);
            return result;
        }

        public async Task<BuildingBlocks.Application.Common.Models.PaginatedResult<CategoryDto>> GetPagedAsync(Guid organizationId, int page, int pageSize)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);
            var countSql = $"SELECT COUNT(*) FROM {TableCategories} WHERE OrganizationId = @OrganizationId";
            var total = await _connection.ExecuteScalarAsync<int>(countSql, new { OrganizationId = organizationId });
            var offset = (page - 1) * pageSize;
            var query = $"SELECT Id, OrganizationId, Name, IsActive FROM {TableCategories} WHERE OrganizationId = @OrganizationId ORDER BY Name OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            var items = await _connection.QueryAsync<CategoryDto>(query, new { OrganizationId = organizationId, Offset = offset, PageSize = pageSize });
            return new BuildingBlocks.Application.Common.Models.PaginatedResult<CategoryDto>(items.ToList(), total, page, pageSize);
        }

        public async Task<CategoryDto?> GetByIdAsync(Guid id)
        {
            var query = "SELECT " +
                        "Id, " +
                        "OrganizationId, " +
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
                        "OrganizationId, " +
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
