using System;
using System.Collections.Generic;

namespace Application.Dtos.Orders
{
    public class OrderWithItemsDto
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public Guid? BranchId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid? CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }
}