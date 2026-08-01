using Application.Dtos.Core;
using Application.QueryRepositories;
using Dapper;
using Infrastructure.Repositories.Queries.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Infrastructure.Repositories.Queries
{
    public class BranchQueryRepository : QueryRepository, IBranchQueryRepository
    {
        public BranchQueryRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<BranchDto?> GetByIdAsync(Guid id)
        {
            var query = $"SELECT id, brand_id AS BrandId, name AS Name FROM {TableBranches} WHERE id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<BranchDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<BranchDto>> GetAllTableAsync()
        {
            var query = $"SELECT id, brand_id AS BrandId, name AS Name FROM {TableBranches}";
            var result = await _connection.QueryAsync<BranchDto>(query);
            return result;
        }

        public async Task<IEnumerable<BranchDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = $"SELECT id, brand_id AS BrandId, name AS Name FROM {TableBranches} WHERE brand_id = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<BranchDto>(query, parameters);
            return result;
        }
    }
}