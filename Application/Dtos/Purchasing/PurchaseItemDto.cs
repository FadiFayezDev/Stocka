using System;

namespace Application.Dtos.Purchasing
{
    public class PurchaseItemDto
    {
        public Guid Id { get; set; }
        public Guid PurchaseId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
    }
}
