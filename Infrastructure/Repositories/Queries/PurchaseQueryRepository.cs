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
            var query = $"SELECT id, brand_id AS BrandId, supplier_id AS SupplierId, purchase_date AS PurchaseDate, total_amount AS TotalAmount FROM {TablePurchases} WHERE id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<PurchaseDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<PurchaseDto>> GetAllTableAsync()
        {
            var query = $"SELECT id, brand_id AS BrandId, supplier_id AS SupplierId, purchase_date AS PurchaseDate, total_amount AS TotalAmount FROM {TablePurchases}";
            var result = await _connection.QueryAsync<PurchaseDto>(query);
            return result;
        }

        public async Task<IEnumerable<PurchaseDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = $"SELECT id, brand_id AS BrandId, supplier_id AS SupplierId, purchase_date AS PurchaseDate, total_amount AS TotalAmount FROM {TablePurchases} WHERE brand_id = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<PurchaseDto>(query, parameters);
            return result;
        }
    }
}
