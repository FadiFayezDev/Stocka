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
            var query = $"SELECT id, purchase_id AS PurchaseId, product_id AS ProductId, quantity AS Quantity, unit_cost AS UnitCost FROM {TablePurchaseItems} WHERE id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<PurchaseItemDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<PurchaseItemDto>> GetAllTableAsync()
        {
            var query = $"SELECT id, purchase_id AS PurchaseId, product_id AS ProductId, quantity AS Quantity, unit_cost AS UnitCost FROM {TablePurchaseItems}";
            var result = await _connection.QueryAsync<PurchaseItemDto>(query);
            return result;
        }

        public async Task<IEnumerable<PurchaseItemDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = $@"SELECT pi.id, pi.purchase_id AS PurchaseId, pi.product_id AS ProductId, pi.quantity AS Quantity, pi.unit_cost AS UnitCost 
                         FROM {TablePurchaseItems} pi
                         INNER JOIN {TablePurchases} p ON pi.purchase_id = p.id
                         WHERE p.brand_id = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<PurchaseItemDto>(query, parameters);
            return result;
        }
    }
}
