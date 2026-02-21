using Domain.Bases;
using Domain.Entities.Core;
using Domain.Enums;
using System;

namespace Domain.Entities.Products
{
    public class StockMovement : IEntity<Guid>
    {
        public Guid Id { get; private set; }

        public Guid ProductId { get; private set; }
        public Guid BatchId { get; private set; }
        public Guid WarehouseId { get; private set; }
        public Guid BrandId { get; private set; }

        public int Quantity { get; private set; }

        public StockMovementType MovementType { get; private set; }

        public StockReferenceType? ReferenceType { get; private set; }
        public Guid? ReferenceId { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public virtual Batch Batch { get; private set; } = null!;
        public virtual Product Product { get; private set; } = null!;
        public virtual Warehouse Warehouse { get; private set; } = null!;
        public virtual Brand Brand { get; private set; } = null!;

        private StockMovement() { } // For EF

        public StockMovement(
            Guid productId,
            Guid batchId,
            Guid warehouseId,
            Guid brandId,
            int quantity,
            StockMovementType movementType,
            StockReferenceType? referenceType = null,
            Guid? referenceId = null)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

            Id = Guid.NewGuid();

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

        // ✅ الكمية الموقعة (دي اللي هتنقذك في الحسابات)
        public int SignedQuantity =>
            IsInbound ? Quantity : -Quantity;

        public Guid GetKey() => Id;

        public void SetKey(Guid key) => Id = key;
    }
}