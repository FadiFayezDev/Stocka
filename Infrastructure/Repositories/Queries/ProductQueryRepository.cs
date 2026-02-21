using Application.Dtos;
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
    public class ProductQueryRepository : QueryRepository, IProductQueryRepository
    {
        public ProductQueryRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<ProductDto?> GetByIdAsync(Guid id)
        {
            var query = $"SELECT id, brand_id AS BrandId, category_id AS CategoryId, name AS Name, barcode AS Barcode FROM {TableProducts} WHERE id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<ProductDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<ProductDto>> GetAllTableAsync()
        {
            var query = $"SELECT id, brand_id AS BrandId, category_id AS CategoryId, name AS Name, barcode AS Barcode FROM {TableProducts}";
            var result = await _connection.QueryAsync<ProductDto>(query);
            return result;
        }

        public async Task<IEnumerable<ProductDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = $"SELECT id, brand_id AS BrandId, category_id AS CategoryId, name AS Name, barcode AS Barcode FROM {TableProducts} WHERE brand_id = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<ProductDto>(query, parameters);
            return result;
        }

        public Task<IEnumerable<ProductsWithWarehouseQuntityDto>> GetProductsWithQuantities(Guid brandId)
        {
            var query = $@"
                SELECT 
                    p.id AS Id, 
                    p.brand_id AS BrandId, 
                    p.category_id AS CategoryId, 
                    p.name AS Name, 
                    p.barcode AS Barcode,
                    COALESCE(SUM(wb.quantity), 0) AS TotalQuantity
                FROM {TableProducts} p
                LEFT JOIN {TableBatches} b ON p.id = b.product_id
                LEFT JOIN {TableWarehouseBatches} wb ON b.id = wb.batch_id
                WHERE p.brand_id = @BrandId
                GROUP BY p.id, p.brand_id, p.category_id, p.name, p.barcode";
            var parameters = new { BrandId = brandId };
            var result = _connection.QueryAsync<ProductsWithWarehouseQuntityDto>(query, parameters);
            return result;
        }
    }
}
