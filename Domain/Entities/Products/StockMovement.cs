using Domain.Bases;
using Domain.Entities.Core;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Products
{
    public partial class StockMovement : IEntity<Guid>
    {
        public Guid GetKey() => Id;

        public void SetKey(Guid key) => Id = key;

        public Guid Id { get; set; }

        public Guid ProductId { get; set; }
        public Guid BatchId { get; set; }
        public Guid WarehouseId { get; set; }
        public Guid BrandId { get; set; }

        public int Quantity { get; set; }
        public StockMovementType MovementType { get; set; }
        public string? ReferenceType { get; set; }
        public Guid? ReferenceId { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Batch Batch { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
        public virtual Warehouse Warehouse { get; set; } = null!;
        public virtual Brand Brand { get; set; } = null!;
    }
}