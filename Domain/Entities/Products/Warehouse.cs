using Domain.Bases;
using Domain.Entities.Core;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Products
{
    public partial class Warehouse : IEntity<Guid>
    {
        public Guid GetKey() => Id;

        public void SetKey(Guid key) => Id = key;

        public Guid Id { get; set; }

        public Guid BranchId { get; set; }
        public Guid BrandId { get; set; }

        public string Name { get; set; } = null!;

        public WarehouseType Type { get; set; }

        public virtual Branch Branch { get; set; } = null!;
        public virtual Brand Brand { get; set; } = null!;

        public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();

        public virtual ICollection<WarehouseBatch> WarehouseBatches { get; set; } = new List<WarehouseBatch>();
    }
}
