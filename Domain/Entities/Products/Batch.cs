using Domain.Bases;
using Domain.Entities.Core;
using Domain.Entities.Purchasing;
using Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Products
{
    public partial class Batch : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }
        public Guid PurchaseItemId { get; set; }
        public Guid BrandId { get; set; }

        public int InitialQuantity { get; set; }
        public int RemainingQuantity { get; private set; }
        public decimal UnitCost { get; set; }
        public DateTime CreatedAt { get; set; }

        private readonly List<OrderItem> _OrderItems = new();
        private readonly List<StockMovement> _stockMovements = new();
        private readonly List<WarehouseBatch> _warehouseBatches = new();

        public virtual Product Product { get; set; } = null!;
        public virtual PurchaseItem PurchaseItem { get; set; } = null!;
        public virtual Brand Brand { get; set; } = null!;

        public virtual ICollection<OrderItem> OrderItems => _OrderItems.AsReadOnly();
        public virtual ICollection<StockMovement> StockMovements => _stockMovements.AsReadOnly();
        public virtual IReadOnlyCollection<WarehouseBatch> WarehouseBatches => _warehouseBatches.AsReadOnly();

        private Batch() { }

        public Batch(Guid productId, Guid purchaseItemId, Guid brandId, int initialQuantity, decimal unitCost)
        {
            Id = Guid.NewGuid();

            if (initialQuantity <= 0)
                throw new ArgumentException("Initial quantity must be greater than zero.", nameof(initialQuantity));

            if (unitCost <= 0)
                throw new ArgumentException("Unit cost must be greater than zero.", nameof(unitCost));

            ProductId = productId;
            PurchaseItemId = purchaseItemId;
            BrandId = brandId;
            InitialQuantity = initialQuantity;
            RemainingQuantity = initialQuantity;
            UnitCost = unitCost;
            CreatedAt = DateTime.UtcNow;
        }

        public void DeductQuantity(int quantityToDeduct)
        {
            if (quantityToDeduct <= 0)
                throw new ArgumentException("Quantity to deduct must be greater than zero.", nameof(quantityToDeduct));

            if (quantityToDeduct > RemainingQuantity)
                throw new InvalidOperationException("Insufficient quantity in batch.");

            RemainingQuantity -= quantityToDeduct;
        }

        public void AddQuantity(int quantityToAdd)
        {
            if (quantityToAdd <= 0)
                throw new ArgumentException("Quantity to add must be greater than zero.", nameof(quantityToAdd));

            RemainingQuantity += quantityToAdd;
        }

        public void DistributeToWarehouse(Guid warehouseId, int quantity)
        {
            _warehouseBatches.Add(new WarehouseBatch(warehouseId, Id, BrandId, quantity));
        }

        public bool IsExhausted => RemainingQuantity == 0;

        public decimal GetTotalCost() => InitialQuantity * UnitCost;

        public Guid GetKey() => Id;

        public void SetKey(Guid key) => Id = key;
    }
}
