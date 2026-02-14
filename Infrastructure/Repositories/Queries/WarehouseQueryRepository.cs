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
            var query = "SELECT Id, BranchId, Name, Type FROM Warehouses WHERE Id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<WarehouseDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<WarehouseDto>> GetAllTableAsync()
        {
            var query = "SELECT Id, BranchId, Name, Type FROM Warehouses";
            var result = await _connection.QueryAsync<WarehouseDto>(query);
            return result;
        }

        public async Task<IEnumerable<WarehouseDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = @"SELECT w.Id, w.BranchId, w.Name, w.Type 
                         FROM Warehouses w
                         INNER JOIN Branches b ON w.BranchId = b.Id
                         WHERE b.BrandId = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<WarehouseDto>(query, parameters);
            return result;
        }
    }
}
