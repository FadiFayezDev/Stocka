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
            var query = "SELECT Id, BrandId, Name, Email, Phone FROM Suppliers WHERE Id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<SupplierDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<SupplierDto>> GetAllTableAsync()
        {
            var query = "SELECT Id, BrandId, Name, Email, Phone FROM Suppliers";
            var result = await _connection.QueryAsync<SupplierDto>(query);
            return result;
        }

        public async Task<IEnumerable<SupplierDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = "SELECT Id, BrandId, Name, Email, Phone FROM Suppliers WHERE BrandId = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<SupplierDto>(query, parameters);
            return result;
        }
    }
}
