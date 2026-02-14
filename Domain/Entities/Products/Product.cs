using Domain.Bases;
using Domain.Entities.Core;
using Domain.Entities.Purchasing;
using Domain.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Products
{
    public partial class Product : IEntity<Guid>
    {
        public Guid GetKey() => Id;

        public void SetKey(Guid key) => Id = key;

        public Guid Id { get; set; }

        public Guid BrandId { get; set; }

        public Guid CategoryId { get; set; }

        public string Name { get; set; } = null!;

        public string? Barcode { get; set; }

        public virtual ICollection<Batch> Batches { get; set; } = new List<Batch>();

        public virtual Brand Brand { get; set; } = null!;

        public virtual ProductCategory Category { get; set; } = null!;

        public virtual ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();

        public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();

        public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
    }
}
