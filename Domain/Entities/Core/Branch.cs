using Domain.Bases;
using Domain.Entities.Products;
using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Domain.Entities.Core;

public partial class Branch : AggregateRoot<BranchId>, IMultiTenantEntity
{
    public BrandId BrandId { get; private set; }

    public string Name { get; private set; } = null!;

    public virtual Brand Brand { get; private set; } = null!;

    private readonly List<WarehouseBranch> _warehouseBranches = new();

    public virtual ICollection<WarehouseBranch> WarehouseBranches => _warehouseBranches.AsReadOnly();

    private Branch() { }

        [SetsRequiredMembers]
        public Branch(BrandId brandId, string name)
    {
        Id = BranchId.New();

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Branch name cannot be empty.", nameof(name));
        
        BrandId = brandId;
        Name = name.Trim();
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Branch name cannot be empty.", nameof(newName));
        
        Name = newName.Trim();
    }

    public void AddWarehouse(Warehouse warehouse)
    {
        if (warehouse == null)
            throw new ArgumentNullException(nameof(warehouse));
        
        if (_warehouseBranches.Any(wb => wb.WarehouseId == warehouse.Id))
            throw new InvalidOperationException("Warehouse already exists in this branch.");
        
        var warehouseBranch = new WarehouseBranch(BrandId, Id, warehouse.Id);
        _warehouseBranches.Add(warehouseBranch);
    }

    public void RemoveWarehouse(WarehouseId warehouseId)
    {
        var warehouseBranch = _warehouseBranches.FirstOrDefault(wb => wb.WarehouseId == warehouseId);
        if (warehouseBranch == null)
            throw new ArgumentException("Warehouse not found in this branch.");
        
        _warehouseBranches.Remove(warehouseBranch);
    }

    Guid IMultiTenantEntity.BrandId
    {
        get => BrandId.Value;
        set => BrandId = new BrandId(value);
    }
}
