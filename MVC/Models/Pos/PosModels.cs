using Application.Dtos.Core;
using Application.Dtos.Orders;
using Application.Dtos.Products;

namespace MVC.Models.Pos
{
    public class PosIndexModel
    {
        public List<ProductDto> Products { get; set; } = new();
        public List<ProductCategoryDto> Categories { get; set; } = new();
        public List<CustomerDto> Customers { get; set; } = new();
        public List<OrderWithItemsDto> RecentOrders { get; set; } = new();
        public List<WarehouseDto> SellingWarehouses { get; set; } = new();
        public Guid? SelectedWarehouseId { get; set; }
        public string? EmployeeName { get; set; }
    }

    public class PosCheckoutModel
    {
        public List<PosItemModel> Items { get; set; } = new();
        public Guid? CustomerId { get; set; }
        public string? Notes { get; set; }
        public Guid? WarehouseId { get; set; }
        public decimal ReceivedAmount { get; set; }
        public decimal Discount { get; set; }
    }

    public class PosItemModel
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class ReceiptModel
    {
        public OrderWithItemsDto Order { get; set; } = new();
        public Dictionary<Guid, string> ProductNames { get; set; } = new();
        public string? CustomerName { get; set; }
        public string? EmployeeName { get; set; }
        public decimal ReceivedAmount { get; set; }
        public decimal Discount { get; set; }
    }
}