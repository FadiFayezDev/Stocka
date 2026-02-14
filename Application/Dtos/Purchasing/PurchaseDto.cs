using System;

namespace Application.Dtos.Purchasing
{
    public class PurchaseDto
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public Guid SupplierId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
