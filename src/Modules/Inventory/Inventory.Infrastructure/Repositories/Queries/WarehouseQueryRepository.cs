using Dapper;
using Inventory.Application.Common.Interfaces;
using Inventory.Application.Repositories.Queries;
using Inventory.Contracts.DTOs.Warehouse;
using Inventory.Infrastructure.Repositories.Queries.Bases;
using Inventory.Primitives.Identifiers;
using BuildingBlocks.Application.Primitives.Identifiers;
using BuildingBlocks.Application.Common.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Repositories.Queries
{
    internal class WarehouseQueryRepository : QueryRepository, IWarehouseQueryRepository
    {
        private const string BaseSelect =
            "SELECT Id, BrandId, Name, Location, Description, IsActive, Type FROM " + TableWarehouses;

        public WarehouseQueryRepository(IInventoryDbConnection connection) : base(connection)
        {
        }

        public async Task<IEnumerable<WarehouseDto>> GetAllAsync(BrandId brandId)
        {
            var sql = $"{BaseSelect} WHERE BrandId = @BrandId ORDER BY Name";
            return await _connection.QueryAsync<WarehouseDto>(sql, new { BrandId = brandId.Value });
        }

        public async Task<PaginatedResult<WarehouseDto>> GetPagedAsync(BrandId brandId, int page, int pageSize)
        {
            return await GetPagedCoreAsync(brandId, null, page, pageSize);
        }

        public async Task<PaginatedResult<WarehouseDto>> GetPagedAsync(BrandId brandId, bool? isActive, int page, int pageSize)
        {
            return await GetPagedCoreAsync(brandId, isActive, page, pageSize);
        }

        private async Task<PaginatedResult<WarehouseDto>> GetPagedCoreAsync(BrandId brandId, bool? isActive, int page, int pageSize)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var isActiveFilter = isActive.HasValue ? " AND IsActive = @IsActive" : string.Empty;
            var countParams = new { BrandId = brandId.Value, IsActive = isActive };

            var countSql = $"SELECT COUNT(*) FROM {TableWarehouses} WHERE BrandId = @BrandId{isActiveFilter}";
            var total = await _connection.ExecuteScalarAsync<int>(countSql, countParams);

            var offset = (page - 1) * pageSize;
            var sql = $"{BaseSelect} WHERE BrandId = @BrandId{isActiveFilter} ORDER BY Name OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            var items = await _connection.QueryAsync<WarehouseDto>(sql, new { BrandId = brandId.Value, IsActive = isActive, Offset = offset, PageSize = pageSize });

            return new PaginatedResult<WarehouseDto>(items.ToList(), total, page, pageSize);
        }

        public async Task<WarehouseDto?> GetByIdAsync(BrandId brandId, WarehouseId id)
        {
            var sql = $"{BaseSelect} WHERE Id = @Id AND BrandId = @BrandId";
            return await _connection.QueryFirstOrDefaultAsync<WarehouseDto>(sql, new { Id = id.Value, BrandId = brandId.Value });
        }

        public async Task<IEnumerable<WarehouseDto>> GetByNameAsync(BrandId brandId, string name)
        {
            // Parameterized contains-search; SQL wildcards from caller input are escaped.
            var pattern = "%" + EscapeLike(name) + "%";
            var sql = $"{BaseSelect} WHERE BrandId = @BrandId AND Name LIKE @Pattern ESCAPE '\\'";
            return await _connection.QueryAsync<WarehouseDto>(sql, new { BrandId = brandId.Value, Pattern = pattern });
        }

        public async Task<WarehouseDto?> GetByExactNameAsync(BrandId brandId, string name)
        {
            var sql = $"{BaseSelect} WHERE BrandId = @BrandId AND Name = @Name";
            return await _connection.QueryFirstOrDefaultAsync<WarehouseDto>(sql, new { BrandId = brandId.Value, Name = name });
        }

        private static string EscapeLike(string value) =>
            value.Replace("\\", "\\\\").Replace("%", "\\%").Replace("_", "\\_").Replace("[", "\\[");
    }
}
