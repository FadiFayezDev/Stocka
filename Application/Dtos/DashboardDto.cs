using System;
using System.Collections.Generic;

namespace Application.Dtos
{
    public class DashboardStatsDto
    {
        public decimal TodaySales { get; set; }
        public decimal TodayPurchases { get; set; }
        public int TodayOrderCount { get; set; }
        public int LowStockCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalExpenses { get; set; }
    }

    public class LowStockProductDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string Barcode { get; set; } = null!;
        public int TotalQuantity { get; set; }
        public int MinimumStock { get; set; }
    }
}