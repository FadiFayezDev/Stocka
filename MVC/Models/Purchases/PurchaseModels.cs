using Application.Dtos.Products;
using Application.Dtos.Purchasing;

namespace MVC.Models.Purchases
{
    public class PurchaseListModel
    {
        public List<PurchaseRowModel> Purchases { get; set; } = new();
        public int TotalOrders { get; set; }
        public decimal TotalOrdered { get; set; }
        public string? StatusFilter { get; set; }
    }

    public class PurchaseRowModel
    {
        public Guid Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Status { get; set; } = null!;
        public string StatusLabel { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public string SupplierName { get; set; } = null!;
        public int ItemCount { get; set; }
        public int TotalUnits { get; set; }
        public int ReceivedUnits { get; set; }
    }

    public class PurchaseCreateModel
    {
        public Guid SupplierId { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.Today;
        public List<SupplierDto> Suppliers { get; set; } = new();
        public List<ProductDto> Products { get; set; } = new();
        public List<PurchaseCreateLineModel> Lines { get; set; } = new();
    }

    public class PurchaseCreateLineModel
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
    }

    public class PurchaseDetailsModel
    {
        public Guid Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Status { get; set; } = null!;
        public string StatusLabel { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public string SupplierName { get; set; } = null!;
        public List<PurchaseItemRowModel> Items { get; set; } = new();
        public bool CanReceive { get; set; }
        public bool CanCancel { get; set; }
        public List<ReceiveWarehouseModel> ReceiveWarehouses { get; set; } = new();
        public ReceivePurchaseFormModel ReceiveForm { get; set; } = new();
    }

    public class ReceiveWarehouseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string TypeLabel { get; set; } = null!;
    }

    public class PurchaseItemRowModel
    {
        public Guid PurchaseItemId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? Barcode { get; set; }
        public int Quantity { get; set; }
        public int ReceivedQuantity { get; set; }
        public int RemainingToReceive { get; set; }
        public decimal UnitCost { get; set; }
    }

    public class ReceivePurchaseFormModel
    {
        public Guid PurchaseId { get; set; }
        public List<ReceiveLineModel> Lines { get; set; } = new();
    }

    public class ReceiveLineModel
    {
        public Guid PurchaseItemId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public List<ReceiveAllocationModel> Allocations { get; set; } = new();
    }

    public class ReceiveAllocationModel
    {
        public Guid WarehouseId { get; set; }
        public string WarehouseName { get; set; } = null!;
        public int Quantity { get; set; }
    }
}