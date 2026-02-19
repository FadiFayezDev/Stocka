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
    public class WarehouseQueryRepository : QueryRepository, IWarehouseQueryRepository
    {
        public WarehouseQueryRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<WarehouseDto?> GetByIdAsync(Guid id)
        {
            var query = $"SELECT id, branch_id AS BranchId, name AS Name, type AS Type FROM Warehouses WHERE id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<WarehouseDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<WarehouseDto>> GetAllTableAsync()
        {
            var query = $"SELECT id, branch_id AS BranchId, name AS Name, type AS Type FROM Warehouses";
            var result = await _connection.QueryAsync<WarehouseDto>(query);
            return result;
        }

        public async Task<IEnumerable<WarehouseDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = $@"SELECT w.id, w.branch_id AS BranchId, w.name AS Name, w.type AS Type 
                         FROM Warehouses w
                         INNER JOIN {TableBranches} b ON w.branch_id = b.id
                         WHERE b.brand_id = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<WarehouseDto>(query, parameters);
            return result;
        }
    }
}
