using System;
using System.Collections.Generic;

namespace Application.Dtos.Purchasing
{
    public class PurchaseWithItemsDto
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public Guid? BranchId { get; set; }
        public Guid SupplierId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Ordered";
        public List<PurchaseItemDto> Items { get; set; } = new();
    }
}