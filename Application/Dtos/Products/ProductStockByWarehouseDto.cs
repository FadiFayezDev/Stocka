using System;

namespace Application.Dtos.Products
{
    public class ProductStockByWarehouseDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string Barcode { get; set; } = null!;
        public Guid WarehouseId { get; set; }
        public string WarehouseName { get; set; } = null!;
        public int Quantity { get; set; }
    }
}