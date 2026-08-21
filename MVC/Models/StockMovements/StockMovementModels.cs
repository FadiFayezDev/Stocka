namespace MVC.Models.StockMovements
{
    public class StockMovementListModel
    {
        public List<StockMovementRowModel> Movements { get; set; } = new();
        public int TotalMovements { get; set; }
        public decimal TotalIn { get; set; }
        public decimal TotalOut { get; set; }
    }

    public class StockMovementRowModel
    {
        public DateTime MovementDate { get; set; }
        public string Type { get; set; } = null!;
        public string TypeLabel { get; set; } = null!;
        public string? ReferenceLabel { get; set; }
        public string ProductName { get; set; } = null!;
        public string WarehouseName { get; set; } = null!;
        public int Quantity { get; set; }
        public bool IsInbound { get; set; }
    }
}