using Domain.Bases;
using Domain.Entities.Core;
using Domain.Enums;
using Domain.Primitives;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

namespace Domain.Entities.Products
{
    public partial class Warehouse : AggregateRoot<WarehouseId>, IMultiTenantEntity
    {
        public BrandId BrandId { get; private set; }

        public string Name { get; private set; } = null!;

        public string Location { get; private set; } = null!;
        public string? Description { get; private set; }

        public bool IsActive { get; private set; }

        public WarehouseType Type { get; private set; }

        private readonly List<StockMovement> _stockMovements = new();
        private readonly List<WarehouseBranch> _warehouseBranches = new();
        private readonly List<WarehouseBatch> _warehouseBatches = new();

        public virtual Brand Brand { get; private set; } = null!;

        public virtual ICollection<StockMovement> StockMovements => _stockMovements.AsReadOnly();
        public virtual ICollection<WarehouseBatch> WarehouseBatches => _warehouseBatches.AsReadOnly();
        public virtual ICollection<WarehouseBranch> WarehouseBranches => _warehouseBranches.AsReadOnly();

        private Warehouse() { }

        [SetsRequiredMembers]
        public Warehouse(BrandId brandId, string name, WarehouseType type, string location, string? description = null)
        {
            Id = WarehouseId.New();

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Warehouse name cannot be empty.", nameof(name));

            if (string.IsNullOrWhiteSpace(location))
                throw new ArgumentException("Warehouse location cannot be empty.", nameof(location));

            BrandId = brandId;
            Name = name.Trim();
            Location = location.Trim();
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
            Type = type;
            IsActive = true;
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

        public void UpdateLocation(string newLocation)
        {
            if (string.IsNullOrWhiteSpace(newLocation))
                throw new ArgumentException("Warehouse location cannot be empty.", nameof(newLocation));

            Location = newLocation.Trim();
        }

        public void UpdateDescription(string? newDescription)
        {
            Description = string.IsNullOrWhiteSpace(newDescription)
                ? null
                : newDescription.Trim();
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
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

        Guid IMultiTenantEntity.BrandId
        {
            get => BrandId.Value;
            set => BrandId = new BrandId(value);
        }
    }
}
