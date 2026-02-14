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
    public class PurchaseItemQueryRepository : QueryRepository, IPurchaseItemQueryRepository
    {
        public PurchaseItemQueryRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<PurchaseItemDto?> GetByIdAsync(Guid id)
        {
            var query = "SELECT Id, PurchaseId, ProductId, Quantity, UnitCost FROM PurchaseItems WHERE Id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<PurchaseItemDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<PurchaseItemDto>> GetAllTableAsync()
        {
            var query = "SELECT Id, PurchaseId, ProductId, Quantity, UnitCost FROM PurchaseItems";
            var result = await _connection.QueryAsync<PurchaseItemDto>(query);
            return result;
        }

        public async Task<IEnumerable<PurchaseItemDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = @"SELECT pi.Id, pi.PurchaseId, pi.ProductId, pi.Quantity, pi.UnitCost 
                         FROM PurchaseItems pi
                         INNER JOIN Purchases p ON pi.PurchaseId = p.Id
                         WHERE p.BrandId = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<PurchaseItemDto>(query, parameters);
            return result;
        }
    }
}
