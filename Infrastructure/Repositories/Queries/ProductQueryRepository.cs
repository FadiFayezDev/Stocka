using Application.Dtos;
using Application.Dtos.Products;
using Application.Common.Interfaces;
using Application.QueryRepositories;
using Dapper;
using Infrastructure.Repositories.Queries.Base;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Infrastructure.Repositories.Queries
{
    public class ProductQueryRepository : QueryRepository, IProductQueryRepository
    {
        private readonly string? _sastoken;
        public ProductQueryRepository(IDbConnection connection, IConfiguration config, ICurrentUserContext? userContext = null) : base(connection, userContext)
        {
            _sastoken = config["AzureBlob:SasToken"] ?? null;
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

        public async Task<ProductDto?> GetProductWithQuantityAsync(Guid productId)
        {
            var query = $@"
                SELECT 
                    p.id AS Id, 
                    p.brand_id AS BrandId, 
                    p.category_id AS CategoryId, 
                    p.name AS Name, 
                    p.barcode AS Barcode,
                    p.selling_price AS SellingPrice,
                    COALESCE(SUM(wb.quantity), 0) AS TotalQuantity,
                    p.image_path AS ImageUrl,
                    p.is_active AS IsActive
                FROM {TableProducts} p
                LEFT JOIN {TableBatches} b ON p.id = b.product_id
                LEFT JOIN {TableWarehouseBatches} wb ON b.id = wb.batch_id
                WHERE p.id = @ProductId
                GROUP BY p.id, p.brand_id, p.category_id, p.name, p.barcode";

            var parameters = new { ProductId = productId };
            var result = await _connection.QuerySingleOrDefaultAsync<ProductDto>(query, parameters);

            if (result != null)
            {
                result.ImageUrl = string.IsNullOrEmpty(result.ImageUrl) ? null : $"{result.ImageUrl}{_sastoken}";
            }

            return result;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsWithQuantities(Guid brandId)
        {
            var query = $@"
                SELECT 
                    p.id AS Id, 
                    p.brand_id AS BrandId, 
                    p.category_id AS CategoryId, 
                    p.name AS Name, 
                    p.barcode AS Barcode,
                    p.selling_price AS SellingPrice,
                    COALESCE(SUM(wb.quantity), 0) AS TotalQuantity,
                    p.image_path AS ImageUrl,
                    p.is_active AS IsActive
                FROM {TableProducts} p
                LEFT JOIN {TableBatches} b ON p.id = b.product_id
                LEFT JOIN {TableWarehouseBatches} wb ON b.id = wb.batch_id
                WHERE p.brand_id = @BrandId
                GROUP BY p.id, p.brand_id, p.category_id, p.name, p.barcode";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<ProductDto>(query, parameters);

            result = result.Select(p => new ProductDto
            {
                Id = p.Id,
                BrandId = p.BrandId,
                CategoryId = p.CategoryId,
                Name = p.Name,
                Barcode = p.Barcode,
                SellingPrice = p.SellingPrice,
                TotalQuantity = p.TotalQuantity,
                ImageUrl = string.IsNullOrEmpty(p.ImageUrl) ? null : $"{p.ImageUrl}{_sastoken}",
                IsActive = p.IsActive
            }).ToList();

            return result;
        }

        public async Task<IEnumerable<ProductStockByWarehouseDto>> GetProductStockByWarehouseAsync(Guid productId)
        {
            var query = $@"
                SELECT 
                    p.id AS ProductId,
                    p.name AS ProductName,
                    p.barcode AS Barcode,
                    w.id AS WarehouseId,
                    w.name AS WarehouseName,
                    COALESCE(wb.quantity, 0) AS Quantity
                FROM {TableProducts} p
                INNER JOIN {TableBatches} b ON p.id = b.product_id
                INNER JOIN {TableWarehouseBatches} wb ON b.id = wb.batch_id
                INNER JOIN {TableWarehouses} w ON wb.warehouse_id = w.id
                WHERE p.id = @ProductId AND wb.quantity > 0";

            var result = await _connection.QueryAsync<ProductStockByWarehouseDto>(query, new { ProductId = productId });
            return result;
        }

        public async Task<IEnumerable<ProductStockByWarehouseDto>> GetAllProductStockByWarehouseAsync(Guid brandId)
        {
            var query = $@"
                SELECT 
                    p.id AS ProductId,
                    p.name AS ProductName,
                    p.barcode AS Barcode,
                    w.id AS WarehouseId,
                    w.name AS WarehouseName,
                    COALESCE(SUM(wb.quantity), 0) AS Quantity
                FROM {TableProducts} p
                INNER JOIN {TableBatches} b ON p.id = b.product_id
                INNER JOIN {TableWarehouseBatches} wb ON b.id = wb.batch_id
                INNER JOIN {TableWarehouses} w ON wb.warehouse_id = w.id
                WHERE p.brand_id = @BrandId AND wb.quantity > 0
                GROUP BY p.id, p.name, p.barcode, w.id, w.name
                ORDER BY p.name, w.name";

            var result = await _connection.QueryAsync<ProductStockByWarehouseDto>(query, new { BrandId = brandId });
            return result;
        }

        public async Task<IEnumerable<LowStockProductDto>> GetLowStockProductsAsync(Guid brandId, int threshold = 10)
        {
            var query = $@"
                SELECT 
                    p.id AS ProductId,
                    p.name AS ProductName,
                    p.barcode AS Barcode,
                    COALESCE(SUM(wb.quantity), 0) AS TotalQuantity,
                    @Threshold AS MinimumStock
                FROM {TableProducts} p
                LEFT JOIN {TableBatches} b ON p.id = b.product_id
                LEFT JOIN {TableWarehouseBatches} wb ON b.id = wb.batch_id
                WHERE p.brand_id = @BrandId AND p.is_active = 1
                GROUP BY p.id, p.name, p.barcode
                HAVING COALESCE(SUM(wb.quantity), 0) <= @Threshold
                ORDER BY TotalQuantity ASC";

            var result = await _connection.QueryAsync<LowStockProductDto>(query, new { BrandId = brandId, Threshold = threshold });
            return result;
        }
    }
}