using Domain.Bases;
using Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Purchasing
{
    public partial class Purchase : IEntity<Guid>
    {
        public Guid GetKey() => Id;

        public void SetKey(Guid key) => Id = key;

        public Guid Id { get; set; }

        public Guid BrandId { get; set; }

        public Guid SupplierId { get; set; }

        public DateTime PurchaseDate { get; set; }

        public decimal TotalAmount { get; set; }

        public virtual Brand Brand { get; set; } = null!;

        public virtual ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();

        public virtual Supplier Supplier { get; set; } = null!;
    }
}
