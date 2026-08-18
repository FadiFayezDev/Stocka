using Domain.Bases;
using Domain.Entities.Core;
using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities.Products;

public partial class WarehouseBatch : Entity<WarehouseBatchId>, IMultiTenantEntity
{
    public WarehouseId WarehouseId { get; private set; }
    public BatchId BatchId { get; private set; }
    public int Quantity { get; private set; }

    public BrandId BrandId { get; private set; }

    public virtual Warehouse Warehouse { get; private set; } = null!;

    private WarehouseBatch() { }

        [SetsRequiredMembers]
        public WarehouseBatch(WarehouseId warehouseId, BatchId batchId, BrandId brandId, int quantity)
    {
        Id = WarehouseBatchId.New();

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

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity < 0)
            throw new ArgumentException("Quantity cannot be negative.", nameof(newQuantity));

        Quantity = newQuantity;
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

    Guid IMultiTenantEntity.BrandId
    {
        get => BrandId.Value;
        set => BrandId = new BrandId(value);
    }
}
