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
    public class PurchaseQueryRepository : QueryRepository, IPurchaseQueryRepository
    {
        public PurchaseQueryRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<PurchaseDto?> GetByIdAsync(Guid id)
        {
            var query = "SELECT Id, BrandId, SupplierId, PurchaseDate, TotalAmount FROM Purchases WHERE Id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<PurchaseDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<PurchaseDto>> GetAllTableAsync()
        {
            var query = "SELECT Id, BrandId, SupplierId, PurchaseDate, TotalAmount FROM Purchases";
            var result = await _connection.QueryAsync<PurchaseDto>(query);
            return result;
        }

        public async Task<IEnumerable<PurchaseDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = "SELECT Id, BrandId, SupplierId, PurchaseDate, TotalAmount FROM Purchases WHERE BrandId = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<PurchaseDto>(query, parameters);
            return result;
        }
    }
}
