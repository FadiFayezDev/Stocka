using System;

namespace Application.Dtos.Products
{
    public class WarehouseBatchDto
    {
        public Guid Id { get; set; }
        public Guid WarehouseId { get; set; }
        public Guid BatchId { get; set; }
        public int Quantity { get; set; }
    }
}
