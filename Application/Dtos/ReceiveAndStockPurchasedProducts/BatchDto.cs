namespace Application.Dtos.ReceiveAndStockPurchasedProducts
{
    public class ItemReceiptDto
    {
        public Guid ProductId { get; set; }
        public int TotalQuantity { get; set; }
        public decimal UnitCost { get; set; }

        public List<BatchReceiptDto> Batches { get; set; } = new();
    }
}