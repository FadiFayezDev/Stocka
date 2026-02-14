using Domain.Bases;
using Domain.Entities.Core;
using Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Sales
{
    public partial class SaleItem : IEntity<Guid>
    {
        public Guid GetKey() => Id;

        public void SetKey(Guid key) => Id = key;

        public Guid Id { get; set; }

        public Guid SaleId { get; set; }

        public Guid ProductId { get; set; }

        public Guid BatchId { get; set; }

        public Guid BrandId { get; set; }

        public int Quantity { get; set => field = value > 0 ? value : throw new ArgumentException("QUANTITY_MUST_BE_GREATER_THAN_ZERO."); }

        public decimal UnitPrice { get; set => field = value > 0 ? value : throw new ArgumentException("UNIT_PRICE_CANNOT_BE_ZERO."); }

        public decimal CostPrice { get; set => field = value > 0 ? value : throw new ArgumentException("COST_PRICE_CANNOT_BE_ZERO."); }

        public virtual Batch Batch { get; set; } = null!;

        public virtual Product Product { get; set; } = null!;

        public virtual Sale Sale { get; set; } = null!;

        public virtual Brand Brand { get; set; } = null!;
    }
}