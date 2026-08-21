using Application.Dtos.Purchasing;
using Application.Common.Interfaces;
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
        public PurchaseQueryRepository(IDbConnection connection, ICurrentUserContext userContext) : base(connection, userContext)
        {
        }

        public async Task<PurchaseDto?> GetByIdAsync(Guid id)
        {
            var query = $@"SELECT id, brand_id AS BrandId, branch_id AS BranchId, supplier_id AS SupplierId, purchase_date AS PurchaseDate, total_amount AS TotalAmount, status AS Status 
                           FROM {TablePurchases}
                           WHERE id = @Id
                             AND brand_id = @BrandId
                             AND (@ApplyBranchFilter = FALSE OR branch_id = @BranchId)";
            
            var parameters = new
            {
                Id = id,
                BrandId = ActiveBrandId,
                ApplyBranchFilter = ApplyBranchScope,
                BranchId = ActiveBranchId
            };
            var result = await _connection.QuerySingleOrDefaultAsync<PurchaseDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<PurchaseDto>> GetAllTableAsync()
        {
            var query = $@"SELECT id, brand_id AS BrandId, branch_id AS BranchId, supplier_id AS SupplierId, purchase_date AS PurchaseDate, total_amount AS TotalAmount, status AS Status 
                           FROM {TablePurchases}
                           WHERE (@BrandId IS NULL OR brand_id = @BrandId)
                             AND (@ApplyBranchFilter = FALSE OR branch_id = @BranchId)";
            var result = await _connection.QueryAsync<PurchaseDto>(query, new
            {
                BrandId = ActiveBrandId,
                ApplyBranchFilter = ApplyBranchScope,
                BranchId = ActiveBranchId
            });
            return result;
        }

        public async Task<IEnumerable<PurchaseDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = $@"SELECT id, brand_id AS BrandId, branch_id AS BranchId, supplier_id AS SupplierId, purchase_date AS PurchaseDate, total_amount AS TotalAmount, status AS Status 
                           FROM {TablePurchases}
                           WHERE brand_id = @BrandId
                             AND (@ApplyBranchFilter = FALSE OR branch_id = @BranchId)";
            var parameters = new
            {
                BrandId = brandId,
                ApplyBranchFilter = ApplyBranchScope,
                BranchId = ActiveBranchId
            };
            var result = await _connection.QueryAsync<PurchaseDto>(query, parameters);
            return result;
        }

        public async Task<PurchaseWithItemsDto?> GetByIdWithItemsAsync(Guid purchaseId)
        {
            var purchaseQuery = $@"SELECT id, brand_id AS BrandId, branch_id AS BranchId, supplier_id AS SupplierId, purchase_date AS PurchaseDate, total_amount AS TotalAmount, status AS Status 
                                   FROM {TablePurchases}
                                   WHERE id = @PurchaseId
                                     AND (@BrandId IS NULL OR brand_id = @BrandId)
                                     AND (@ApplyBranchFilter = FALSE OR branch_id = @BranchId)";

            var purchase = await _connection.QuerySingleOrDefaultAsync<PurchaseWithItemsDto>(purchaseQuery, new
            {
                PurchaseId = purchaseId,
                BrandId = ActiveBrandId,
                ApplyBranchFilter = ApplyBranchScope,
                BranchId = ActiveBranchId
            });

            if (purchase == null) return null;

            var itemsQuery = $@"SELECT id, purchase_id AS PurchaseId, product_id AS ProductId, quantity AS Quantity, received_quantity AS ReceivedQuantity, unit_cost AS UnitCost 
                                FROM {TablePurchaseItems} 
                                WHERE purchase_id = @PurchaseId";

            var items = await _connection.QueryAsync<PurchaseItemDto>(itemsQuery, new { PurchaseId = purchaseId });
            purchase.Items = items.ToList();

            return purchase;
        }

        public async Task<IEnumerable<PurchaseWithItemsDto>> GetAllWithItemsAsync()
        {
            var purchasesQuery = $@"SELECT id, brand_id AS BrandId, branch_id AS BranchId, supplier_id AS SupplierId, purchase_date AS PurchaseDate, total_amount AS TotalAmount, status AS Status 
                                    FROM {TablePurchases}
                                    WHERE (@BrandId IS NULL OR brand_id = @BrandId)
                                      AND (@ApplyBranchFilter = FALSE OR branch_id = @BranchId)";

            var purchases = (await _connection.QueryAsync<PurchaseWithItemsDto>(purchasesQuery, new
            {
                BrandId = ActiveBrandId,
                ApplyBranchFilter = ApplyBranchScope,
                BranchId = ActiveBranchId
            })).ToList();

            if (!purchases.Any()) return purchases;

            var purchaseIds = purchases.Select(p => p.Id).ToList();
            var itemsQuery = $@"SELECT id, purchase_id AS PurchaseId, product_id AS ProductId, quantity AS Quantity, received_quantity AS ReceivedQuantity, unit_cost AS UnitCost 
                                FROM {TablePurchaseItems} 
                                WHERE purchase_id = ANY(@PurchaseIds)";

            var allItems = await _connection.QueryAsync<PurchaseItemDto>(itemsQuery, new { PurchaseIds = purchaseIds });
            var itemsByPurchase = allItems.GroupBy(i => i.PurchaseId).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var purchase in purchases)
            {
                purchase.Items = itemsByPurchase.TryGetValue(purchase.Id, out var items) ? items : new List<PurchaseItemDto>();
            }

            return purchases;
        }

        public async Task<IEnumerable<PurchaseWithItemsDto>> GetAllWithItemsByBrandIdAsync(Guid brandId)
        {
            var purchasesQuery = $@"SELECT id, brand_id AS BrandId, branch_id AS BranchId, supplier_id AS SupplierId, purchase_date AS PurchaseDate, total_amount AS TotalAmount, status AS Status 
                                    FROM {TablePurchases}
                                    WHERE brand_id = @BrandId
                                      AND (@ApplyBranchFilter = FALSE OR branch_id = @BranchId)";

            var purchases = (await _connection.QueryAsync<PurchaseWithItemsDto>(purchasesQuery, new
            {
                BrandId = brandId,
                ApplyBranchFilter = ApplyBranchScope,
                BranchId = ActiveBranchId
            })).ToList();

            if (!purchases.Any()) return purchases;

            var purchaseIds = purchases.Select(p => p.Id).ToList();
            var itemsQuery = $@"SELECT id, purchase_id AS PurchaseId, product_id AS ProductId, quantity AS Quantity, received_quantity AS ReceivedQuantity, unit_cost AS UnitCost 
                                FROM {TablePurchaseItems} 
                                WHERE purchase_id = ANY(@PurchaseIds)";

            var allItems = await _connection.QueryAsync<PurchaseItemDto>(itemsQuery, new { PurchaseIds = purchaseIds });
            var itemsByPurchase = allItems.GroupBy(i => i.PurchaseId).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var purchase in purchases)
            {
                purchase.Items = itemsByPurchase.TryGetValue(purchase.Id, out var items) ? items : new List<PurchaseItemDto>();
            }

            return purchases;
        }
    }
}
