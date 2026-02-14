using System;

namespace Application.Dtos.Products
{
    public class BatchDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid PurchaseItemId { get; set; }
        public int InitialQuantity { get; set; }
        public int RemainingQuantity { get; set; }
        public decimal UnitCost { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
