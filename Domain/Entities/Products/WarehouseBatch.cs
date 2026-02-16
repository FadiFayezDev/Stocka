using Domain.Bases;
using Domain.Entities.Core;
using System;
using System.Collections.Generic;

namespace Domain.Entities.Products;

public partial class WarehouseBatch : IEntity<Guid>
{
    public Guid Id { get; set; }

    public Guid WarehouseId { get; set; }
    public Guid BatchId { get; set; }
    public int Quantity { get; set; }

    public Guid BrandId { get; set; }

    public virtual Warehouse Warehouse { get; set; } = null!;
    public virtual Batch Batch { get; set; } = null!;

    public virtual Brand Brand { get; set; } = null!;

    private WarehouseBatch() { }

    public WarehouseBatch(Guid warehouseId, Guid batchId, Guid brandId, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

        WarehouseId = warehouseId;
        BatchId = batchId;
        BrandId = brandId;
        Quantity = quantity;
    }

    public void AddQuantity(int quantityToAdd)
    {
        if (quantityToAdd <= 0)
            throw new ArgumentException("Quantity to add must be greater than zero.", nameof(quantityToAdd));

        Quantity += quantityToAdd;
    }

    public void DeductQuantity(int quantityToDeduct)
    {
        if (quantityToDeduct <= 0)
            throw new ArgumentException("Quantity to deduct must be greater than zero.", nameof(quantityToDeduct));

        if (quantityToDeduct > Quantity)
            throw new InvalidOperationException("Insufficient quantity in warehouse batch.");

        Quantity -= quantityToDeduct;
    }

    public bool IsEmpty => Quantity == 0;

    public Guid GetKey() => Id;

    public void SetKey(Guid key) => Id = key;
}