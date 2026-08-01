using Application.Dtos;
using Application.Common.Interfaces;
using Application.QueryRepositories;
using Dapper;
using Infrastructure.Repositories.Queries.Base;
using System.Data;

namespace Infrastructure.Repositories.Queries
{
    public class DashboardQueryRepository : QueryRepository, IDashboardQueryRepository
    {
        public DashboardQueryRepository(IDbConnection connection, ICurrentUserContext? userContext = null) : base(connection, userContext)
        {
        }

        public async Task<DashboardStatsDto> GetDashboardStatsAsync(Guid brandId)
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            var todaySalesQuery = $@"
                SELECT COALESCE(SUM(total_amount), 0) 
                FROM {TableOrders} 
                WHERE brand_id = @BrandId 
                  AND order_date >= @Today 
                  AND order_date < @Tomorrow";

            var todayPurchasesQuery = $@"
                SELECT COALESCE(SUM(total_amount), 0) 
                FROM {TablePurchases} 
                WHERE brand_id = @BrandId 
                  AND purchase_date >= @Today 
                  AND purchase_date < @Tomorrow";

            var todayOrderCountQuery = $@"
                SELECT COUNT(*) 
                FROM {TableOrders} 
                WHERE brand_id = @BrandId 
                  AND order_date >= @Today 
                  AND order_date < @Tomorrow";

            var lowStockQuery = $@"
                SELECT COUNT(DISTINCT p.id)
                FROM {TableProducts} p
                LEFT JOIN {TableBatches} b ON p.id = b.product_id
                LEFT JOIN {TableWarehouseBatches} wb ON b.id = wb.batch_id
                WHERE p.brand_id = @BrandId AND p.is_active = 1
                GROUP BY p.id
                HAVING COALESCE(SUM(wb.quantity), 0) <= 10";

            var totalRevenueQuery = $@"
                SELECT COALESCE(SUM(total_amount), 0) 
                FROM {TableOrders} 
                WHERE brand_id = @BrandId";

            var totalExpensesQuery = $@"
                SELECT COALESCE(SUM(amount), 0) 
                FROM {TableExpenses} 
                WHERE brand_id = @BrandId";

            var todaySales = await _connection.ExecuteScalarAsync<decimal>(todaySalesQuery, new { BrandId = brandId, Today = today, Tomorrow = tomorrow });
            var todayPurchases = await _connection.ExecuteScalarAsync<decimal>(todayPurchasesQuery, new { BrandId = brandId, Today = today, Tomorrow = tomorrow });
            var todayOrderCount = await _connection.ExecuteScalarAsync<int>(todayOrderCountQuery, new { BrandId = brandId, Today = today, Tomorrow = tomorrow });
            var lowStockCount = (await _connection.QueryAsync<int>(lowStockQuery, new { BrandId = brandId })).Count();
            var totalRevenue = await _connection.ExecuteScalarAsync<decimal>(totalRevenueQuery, new { BrandId = brandId });
            var totalExpenses = await _connection.ExecuteScalarAsync<decimal>(totalExpensesQuery, new { BrandId = brandId });

            return new DashboardStatsDto
            {
                TodaySales = todaySales,
                TodayPurchases = todayPurchases,
                TodayOrderCount = todayOrderCount,
                LowStockCount = lowStockCount,
                TotalRevenue = totalRevenue,
                TotalExpenses = totalExpenses
            };
        }
    }
}