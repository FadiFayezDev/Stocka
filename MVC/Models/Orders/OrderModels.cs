namespace MVC.Models.Orders
{
    public class OrderListModel
    {
        public List<OrderRowModel> Orders { get; set; } = new();
        public int TotalOrders { get; set; }
        public decimal TotalSales { get; set; }
    }

    public class OrderRowModel
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public string? BranchName { get; set; }
        public int ItemCount { get; set; }
        public int TotalUnits { get; set; }
    }

    public class OrderDetailsModel
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public string? BranchName { get; set; }
        public int TotalUnits { get; set; }
        public List<OrderItemRowModel> Items { get; set; } = new();
    }

    public class OrderItemRowModel
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? Barcode { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal CostPrice { get; set; }
        public decimal LineTotal => Quantity * UnitPrice;
        public decimal LineProfit => (UnitPrice - CostPrice) * Quantity;
    }
}