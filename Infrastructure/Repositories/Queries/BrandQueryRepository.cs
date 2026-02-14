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
    public class BrandQueryRepository : QueryRepository, IBrandQueryRepository
    {
        public BrandQueryRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<BrandDto?> GetByIdAsync(Guid id)
        {
            var query = "SELECT Id, Name, Slug, CreatedAt FROM Brands WHERE Id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<BrandDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<BrandDto>> GetAllTableAsync()
        {
            var query = "SELECT Id, Name, Slug, CreatedAt FROM Brands";
            var result = await _connection.QueryAsync<BrandDto>(query);
            return result;
        }

        public async Task<IEnumerable<BrandDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = "SELECT Id, Name, Slug, CreatedAt FROM Brands WHERE Id = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<BrandDto>(query, parameters);
            return result;
        }
    }
}
