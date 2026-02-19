using Domain.Bases;
using Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities.Core;

public partial class Branch : IEntity<Guid>
{
    public Guid Id { get; set; }

    public Guid BrandId { get; set; }

    public string Name { get; set; } = null!;

    public virtual Brand Brand { get; set; } = null!;

    private readonly List<Warehouse> _warehouses = new();
    public virtual ICollection<Warehouse> Warehouses => _warehouses.AsReadOnly();

    private Branch() { }

    public Branch(Guid brandId, string name)
    {
        Id = Guid.NewGuid();

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
        
        if (warehouse.BranchId != Id)
            throw new ArgumentException("Warehouse does not belong to this branch.");
        
        if (_warehouses.Any(w => w.Id == warehouse.Id))
            throw new InvalidOperationException("Warehouse already exists in this branch.");
        
        _warehouses.Add(warehouse);
    }

    public void RemoveWarehouse(Guid warehouseId)
    {
        var warehouse = _warehouses.FirstOrDefault(w => w.Id == warehouseId);
        if (warehouse == null)
            throw new ArgumentException("Warehouse not found in this branch.");
        
        _warehouses.Remove(warehouse);
    }

    public Guid GetKey()
    {
        return Id;
    }

    public void SetKey(Guid id)
    {
        Id = id;
    }
}
