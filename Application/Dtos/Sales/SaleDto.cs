using System;

namespace Application.Dtos.Sales
{
    public class SaleDto
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid? CustomerId { get; set; }
        public DateTime SaleDate { get; set; }
        public string Status { get; set; } = null!;
        public decimal TotalAmount { get; set; }
    }
}
