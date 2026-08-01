using Application.Dtos.Purchasing;
using Application.QueryRepositories;
using Dapper;
using Infrastructure.Repositories.Queries.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Infrastructure.Repositories.Queries
{
    public class SupplierQueryRepository : QueryRepository, ISupplierQueryRepository
    {
        public SupplierQueryRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<SupplierDto?> GetByIdAsync(Guid id)
        {
            var query = $"SELECT id, brand_id AS BrandId, name AS Name, email AS Email, phone AS Phone, address AS Address FROM {TableSuppliers} WHERE id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<SupplierDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<SupplierDto>> GetAllTableAsync()
        {
            var query = $"SELECT id, brand_id AS BrandId, name AS Name, email AS Email, phone AS Phone, address AS Address FROM {TableSuppliers}";
            var result = await _connection.QueryAsync<SupplierDto>(query);
            return result;
        }

        public async Task<IEnumerable<SupplierDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = $"SELECT id, brand_id AS BrandId, name AS Name, email AS Email, phone AS Phone, address AS Address FROM {TableSuppliers} WHERE brand_id = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<SupplierDto>(query, parameters);
            return result;
        }
    }
}
