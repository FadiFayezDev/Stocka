using Application.Dtos;
using Application.Dtos.Products;
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
        public ProductQueryRepository(IDbConnection connection, IConfiguration config) : base(connection)
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

        public async Task<ProductDto?> GetProductsWithQuantityAsync(Guid productId)
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
    }
}