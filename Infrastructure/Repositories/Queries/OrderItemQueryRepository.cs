using Application.Dtos.Orders;
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
    public class OrderItemQueryRepository : QueryRepository, IOrderItemQueryRepository
    {
        public OrderItemQueryRepository(IDbConnection connection, ICurrentUserContext userContext) : base(connection, userContext)
        {
        }

        public async Task<OrderItemDto?> GetByIdAsync(Guid id)
        {
            var query = $@"SELECT si.id, si.order_id AS OrderId, si.product_id AS ProductId, si.batch_id AS BatchId, si.quantity AS Quantity, si.unit_price AS UnitPrice, si.cost_price AS CostPrice 
                           FROM {TableOrderItems} si
                           INNER JOIN {TableOrders} s ON si.order_id = s.id
                           WHERE si.id = @Id
                             AND (@BrandId IS NULL OR s.brand_id = @BrandId)
                             AND (@ApplyBranchFilter = FALSE OR s.branch_id = @BranchId)";
            var parameters = new
            {
                Id = id,
                BrandId = ActiveBrandId,
                ApplyBranchFilter = ApplyBranchScope,
                BranchId = ActiveBranchId
            };
            var result = await _connection.QuerySingleOrDefaultAsync<OrderItemDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<OrderItemDto>> GetAllTableAsync()
        {
            var query = $@"SELECT si.id, si.order_id AS OrderId, si.product_id AS ProductId, si.batch_id AS BatchId, si.quantity AS Quantity, si.unit_price AS UnitPrice, si.cost_price AS CostPrice 
                           FROM {TableOrderItems} si
                           INNER JOIN {TableOrders} s ON si.order_id = s.id
                           WHERE (@BrandId IS NULL OR s.brand_id = @BrandId)
                             AND (@ApplyBranchFilter = FALSE OR s.branch_id = @BranchId)";
            var result = await _connection.QueryAsync<OrderItemDto>(query, new
            {
                BrandId = ActiveBrandId,
                ApplyBranchFilter = ApplyBranchScope,
                BranchId = ActiveBranchId
            });
            return result;
        }

        public async Task<IEnumerable<OrderItemDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = $@"SELECT si.id, si.order_id AS OrderId, si.product_id AS ProductId, si.batch_id AS BatchId, si.quantity AS Quantity, si.unit_price AS UnitPrice, si.cost_price AS CostPrice 
                         FROM {TableOrderItems} si
                         INNER JOIN {TableOrders} s ON si.order_id = s.id
                         WHERE s.brand_id = @BrandId
                           AND (@ApplyBranchFilter = FALSE OR s.branch_id = @BranchId)";
            var parameters = new
            {
                BrandId = brandId,
                ApplyBranchFilter = ApplyBranchScope,
                BranchId = ActiveBranchId
            };
            var result = await _connection.QueryAsync<OrderItemDto>(query, parameters);
            return result;
        }
    }
}
