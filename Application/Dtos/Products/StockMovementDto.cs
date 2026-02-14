using System;

namespace Application.Dtos.Products
{
    public class StockMovementDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid BatchId { get; set; }
        public Guid WarehouseId { get; set; }
        public int Quantity { get; set; }
        public string MovementType { get; set; } = null!;
        public string? ReferenceType { get; set; }
        public Guid? ReferenceId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
