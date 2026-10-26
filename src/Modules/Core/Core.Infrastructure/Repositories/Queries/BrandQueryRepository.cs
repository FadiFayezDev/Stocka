using BuildingBlocks.Application.Primitives.Identifiers;
using Core.Application.DTOs.Brand;
using Core.Application.Repositories.Queries.Brand;
using Core.Infrastructure.Repositories.Queries.Bases;
using System.Data;
using Dapper;

namespace Core.Infrastructure.Repositories.Queries
{
    internal class BrandQueryRepository : QueryRepository, IBrandQueryRepository
    {
        public BrandQueryRepository(IDbConnection connection) : base(connection)
        {

        }

        public async Task<BrandDto?> GetBrandByIdAsync(BrandId id)
        {
            string query = "SELECT " +
                           "Id, " +
                           "Name, " +
                           "Slug " +
                           $"FROM {TableBrands} " +
                           "WHERE Id = @Id";
            var parameters = new { Id = id.Value };
            var brand = await _connection.QueryFirstOrDefaultAsync<BrandDto>(query, parameters);
            return brand;
        }
    }
}