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
    public class OrderQueryRepository : QueryRepository, IOrderQueryRepository
    {
        public OrderQueryRepository(IDbConnection connection, ICurrentUserContext userContext) : base(connection, userContext)
        {
        }

        public async Task<OrderDto?> GetByIdAsync(Guid id)
        {
            var query = $@"SELECT id, brand_id AS BrandId, branch_id AS BranchId, employee_id AS EmployeeId, customer_id AS CustomerId, order_date AS OrderDate, status AS Status, total_amount AS TotalAmount 
                           FROM {TableOrders}
                           WHERE id = @Id
                             AND (@BrandId IS NULL OR brand_id = @BrandId)
                             AND (@ApplyBranchFilter = FALSE OR branch_id = @BranchId)";
            var parameters = new
            {
                Id = id,
                BrandId = ActiveBrandId,
                ApplyBranchFilter = ApplyBranchScope,
                BranchId = ActiveBranchId
            };
            var result = await _connection.QuerySingleOrDefaultAsync<OrderDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<OrderDto>> GetAllTableAsync()
        {
            var query = $@"SELECT id, brand_id AS BrandId, branch_id AS BranchId, employee_id AS EmployeeId, customer_id AS CustomerId, order_date AS OrderDate, status AS Status, total_amount AS TotalAmount 
                           FROM {TableOrders}
                           WHERE (@BrandId IS NULL OR brand_id = @BrandId)
                             AND (@ApplyBranchFilter = FALSE OR branch_id = @BranchId)";
            var result = await _connection.QueryAsync<OrderDto>(query, new
            {
                BrandId = ActiveBrandId,
                ApplyBranchFilter = ApplyBranchScope,
                BranchId = ActiveBranchId
            });
            return result;
        }

        public async Task<IEnumerable<OrderDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = $@"SELECT id, brand_id AS BrandId, branch_id AS BranchId, employee_id AS EmployeeId, customer_id AS CustomerId, order_date AS OrderDate, status AS Status, total_amount AS TotalAmount 
                           FROM {TableOrders}
                           WHERE brand_id = @BrandId
                             AND (@ApplyBranchFilter = FALSE OR branch_id = @BranchId)";
            var parameters = new
            {
                BrandId = brandId,
                ApplyBranchFilter = ApplyBranchScope,
                BranchId = ActiveBranchId
            };
            var result = await _connection.QueryAsync<OrderDto>(query, parameters);
            return result;
        }

        public async Task<OrderWithItemsDto?> GetByIdWithItemsAsync(Guid orderId)
        {
            var orderQuery = $@"SELECT id, brand_id AS BrandId, branch_id AS BranchId, employee_id AS EmployeeId, customer_id AS CustomerId, order_date AS OrderDate, status AS Status, total_amount AS TotalAmount 
                               FROM {TableOrders}
                               WHERE id = @OrderId
                                 AND (@BrandId IS NULL OR brand_id = @BrandId)
                                 AND (@ApplyBranchFilter = FALSE OR branch_id = @BranchId)";

            var order = await _connection.QuerySingleOrDefaultAsync<OrderWithItemsDto>(orderQuery, new
            {
                OrderId = orderId,
                BrandId = ActiveBrandId,
                ApplyBranchFilter = ApplyBranchScope,
                BranchId = ActiveBranchId
            });

            if (order == null) return null;

            var itemsQuery = $@"SELECT id, order_id AS OrderId, product_id AS ProductId, batch_id AS BatchId, quantity AS Quantity, unit_price AS UnitPrice, cost_price AS CostPrice 
                                FROM {TableOrderItems} 
                                WHERE order_id = @OrderId";

            var items = await _connection.QueryAsync<OrderItemDto>(itemsQuery, new { OrderId = orderId });
            order.Items = items.ToList();

            return order;
        }

        public async Task<IEnumerable<OrderWithItemsDto>> GetAllWithItemsAsync()
        {
            var ordersQuery = $@"SELECT id, brand_id AS BrandId, branch_id AS BranchId, employee_id AS EmployeeId, customer_id AS CustomerId, order_date AS OrderDate, status AS Status, total_amount AS TotalAmount 
                                 FROM {TableOrders}
                                 WHERE (@BrandId IS NULL OR brand_id = @BrandId)
                                   AND (@ApplyBranchFilter = FALSE OR branch_id = @BranchId)";

            var orders = (await _connection.QueryAsync<OrderWithItemsDto>(ordersQuery, new
            {
                BrandId = ActiveBrandId,
                ApplyBranchFilter = ApplyBranchScope,
                BranchId = ActiveBranchId
            })).ToList();

            if (!orders.Any()) return orders;

            var orderIds = orders.Select(o => o.Id).ToList();
            var itemsQuery = $@"SELECT id, order_id AS OrderId, product_id AS ProductId, batch_id AS BatchId, quantity AS Quantity, unit_price AS UnitPrice, cost_price AS CostPrice 
                                FROM {TableOrderItems} 
                                WHERE order_id = ANY(@OrderIds)";

            var allItems = await _connection.QueryAsync<OrderItemDto>(itemsQuery, new { OrderIds = orderIds });
            var itemsByOrder = allItems.GroupBy(i => i.OrderId).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var order in orders)
            {
                order.Items = itemsByOrder.TryGetValue(order.Id, out var items) ? items : new List<OrderItemDto>();
            }

            return orders;
        }

        public async Task<IEnumerable<OrderWithItemsDto>> GetAllWithItemsByBrandIdAsync(Guid brandId)
        {
            var ordersQuery = $@"SELECT id, brand_id AS BrandId, branch_id AS BranchId, employee_id AS EmployeeId, customer_id AS CustomerId, order_date AS OrderDate, status AS Status, total_amount AS TotalAmount 
                                 FROM {TableOrders}
                                 WHERE brand_id = @BrandId
                                   AND (@ApplyBranchFilter = FALSE OR branch_id = @BranchId)";

            var orders = (await _connection.QueryAsync<OrderWithItemsDto>(ordersQuery, new
            {
                BrandId = brandId,
                ApplyBranchFilter = ApplyBranchScope,
                BranchId = ActiveBranchId
            })).ToList();

            if (!orders.Any()) return orders;

            var orderIds = orders.Select(o => o.Id).ToList();
            var itemsQuery = $@"SELECT id, order_id AS OrderId, product_id AS ProductId, batch_id AS BatchId, quantity AS Quantity, unit_price AS UnitPrice, cost_price AS CostPrice 
                                FROM {TableOrderItems} 
                                WHERE order_id = ANY(@OrderIds)";

            var allItems = await _connection.QueryAsync<OrderItemDto>(itemsQuery, new { OrderIds = orderIds });
            var itemsByOrder = allItems.GroupBy(i => i.OrderId).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var order in orders)
            {
                order.Items = itemsByOrder.TryGetValue(order.Id, out var items) ? items : new List<OrderItemDto>();
            }

            return orders;
        }
    }
}
