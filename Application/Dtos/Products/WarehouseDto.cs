using System;

namespace Application.Dtos.Products
{
    public class WarehouseDto
    {
        public Guid Id { get; set; }
        public Guid BranchId { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
    }
}
