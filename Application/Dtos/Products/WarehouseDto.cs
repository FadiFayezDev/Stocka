using System;

namespace Application.Dtos.Products
{
    public class WarehouseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string? Description { get; set; }
        public string Type { get; set; } = null!;
    }
}