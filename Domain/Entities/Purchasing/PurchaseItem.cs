using Domain.Bases;
using Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities.Purchasing
{
    public partial class PurchaseItem : IEntity<Guid>
    {
        public Guid GetKey() => Id;

        public void SetKey(Guid key) => Id = key;

        public Guid Id { get; set; }

        public Guid PurchaseId { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set => field = value > 0 ? value : throw new ArgumentException("QUANTITY_MUST_BE_GREATER_THAN_ZERO."); }

        public decimal UnitCost { get; set => field = value > 0 ? value : throw new ArgumentException("UNIT_COST_CANNOT_BE_ZERO."); }

        public virtual ICollection<Batch> Batches { get; set; } = new List<Batch>();

        public virtual Product Product { get; set; } = null!;

        public virtual Purchase Purchase { get; set; } = null!;

        [NotMapped]
        public decimal TotalCost => Quantity * UnitCost;
    }
}
