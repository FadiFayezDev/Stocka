using Domain.Bases;
using Domain.Entities.Core;
using Domain.Entities.Purchasing;
using Domain.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Products
{
    public partial class Batch : IEntity<Guid>
    {
        public Guid GetKey() => Id;

        public void SetKey(Guid key) => Id = key;

        public Guid Id { get; set; }

        public Guid ProductId { get; set; }
        public Guid PurchaseItemId { get; set; }
        public Guid BrandId { get; set; }

        public int InitialQuantity { get; set; }
        public int RemainingQuantity { get; set; }
        public decimal UnitCost { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual PurchaseItem PurchaseItem { get; set; } = null!;
        public virtual Brand Brand { get; set; } = null!;

        public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
        public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
        public virtual ICollection<WarehouseBatch> WarehouseBatches { get; set; } = new List<WarehouseBatch>();
    }
}
