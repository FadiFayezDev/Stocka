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
        public Guid Id { get; set; }

        public Guid BranchId { get; set; }
        public Guid BrandId { get; set; }

        public string Name { get; set; } = null!;

        public WarehouseType Type { get; set; }

        private readonly List<StockMovement> _stockMovements = new();
        private readonly List<WarehouseBatch> _warehouseBatches = new();

        public virtual Branch Branch { get; set; } = null!;
        public virtual Brand Brand { get; set; } = null!;

        public virtual ICollection<StockMovement> StockMovements => _stockMovements.AsReadOnly();
        public virtual ICollection<WarehouseBatch> WarehouseBatches => _warehouseBatches.AsReadOnly();

        private Warehouse() { }

        public Warehouse(Guid branchId, Guid brandId, string name, WarehouseType type)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Warehouse name cannot be empty.", nameof(name));

            BranchId = branchId;
            BrandId = brandId;
            Name = name.Trim();
            Type = type;
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Warehouse name cannot be empty.", nameof(newName));

            Name = newName.Trim();
        }

        public void ChangeType(WarehouseType newType)
        {
            Type = newType;
        }

        public void AddStockMovement(StockMovement movement)
        {
            if (movement == null)
                throw new ArgumentNullException(nameof(movement));

            if (movement.WarehouseId != Id)
                throw new ArgumentException("Stock movement does not belong to this warehouse.");

            if (_stockMovements.Any(m => m.Id == movement.Id))
                throw new InvalidOperationException("Stock movement already added.");

            _stockMovements.Add(movement);
        }

        public Guid GetKey() => Id;

        public void SetKey(Guid key) => Id = key;
    }
}
