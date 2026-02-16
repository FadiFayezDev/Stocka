using System;

namespace Application.Dtos.Orders
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid? CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = null!;
        public decimal TotalAmount { get; set; }
    }
}
