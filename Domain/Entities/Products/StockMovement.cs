using Domain.Bases;
using Domain.Entities.Core;
using Domain.Enums;
using Domain.Primitives;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities.Products
{
    public class StockMovement : AggregateRoot<StockMovementId>, IMultiTenantEntity
    {
        public ProductId ProductId { get; private set; }
        public BatchId BatchId { get; private set; }
        public WarehouseId WarehouseId { get; private set; }
        public BrandId BrandId { get; private set; }

        public int Quantity { get; private set; }

        public StockMovementType MovementType { get; private set; }

        public StockReferenceType? ReferenceType { get; private set; }
        public Guid? ReferenceId { get; private set; }

        public DateTime CreatedAt { get; private set; }

        private StockMovement() { } // For EF

        [SetsRequiredMembers]
        public StockMovement(
            ProductId productId,
            BatchId batchId,
            WarehouseId warehouseId,
            BrandId brandId,
            int quantity,
            StockMovementType movementType,
            StockReferenceType? referenceType = null,
            Guid? referenceId = null)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

            Id = StockMovementId.New();

            ProductId = productId;
            BatchId = batchId;
            WarehouseId = warehouseId;
            BrandId = brandId;

            Quantity = quantity;
            MovementType = movementType;

            ReferenceType = referenceType;
            ReferenceId = referenceId;

            CreatedAt = DateTime.UtcNow;
        }

        // ✅ اتجاه الحركة
        public bool IsInbound =>
            MovementType == StockMovementType.PurchaseIn ||
            MovementType == StockMovementType.TransferIn ||
            MovementType == StockMovementType.AdjustmentIn ||
            MovementType == StockMovementType.OrderReturn;

        public bool IsOutbound =>
            MovementType == StockMovementType.SaleOut ||
            MovementType == StockMovementType.TransferOut ||
            MovementType == StockMovementType.AdjustmentOut;

        public int SignedQuantity =>
            IsInbound ? Quantity : -Quantity;

        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(newQuantity));

            Quantity = newQuantity;
        }

        public void UpdateReference(StockReferenceType? referenceType, Guid? referenceId)
        {
            ReferenceType = referenceType;
            ReferenceId = referenceId;
        }

        Guid IMultiTenantEntity.BrandId
        {
            get => BrandId.Value;
            set => BrandId = new BrandId(value);
        }
    }
}
