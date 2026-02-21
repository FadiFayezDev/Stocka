namespace Application.Dtos.ReceiveAndStockPurchasedProducts
{
    public class BatchReceiptDto
    {
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }

        public List<WarehouseDistributionDto> Warehouses { get; set; } = new();
    }
}