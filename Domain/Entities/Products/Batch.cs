using Domain.Bases;
using Domain.Entities.Core;
using Domain.Entities.Purchasing;
using Domain.Entities.Orders;
using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Domain.Entities.Products
{
    public partial class Batch : AggregateRoot<BatchId>, IMultiTenantEntity
    {
        public ProductId ProductId { get; private set; }
        public PurchaseItemId PurchaseItemId { get; private set; }
        public BrandId BrandId { get; private set; }

        public int InitialQuantity { get; private set; }
        public int RemainingQuantity { get; private set; }
        public decimal UnitCost { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private Batch() { }

        [SetsRequiredMembers]
        public Batch(ProductId productId, PurchaseItemId purchaseItemId, BrandId brandId, int initialQuantity, decimal unitCost)
        {
            Id = BatchId.New();

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

        public void UpdateUnitCost(decimal newUnitCost)
        {
            if (newUnitCost <= 0)
                throw new ArgumentException("Unit cost must be greater than zero.", nameof(newUnitCost));

            UnitCost = newUnitCost;
        }

        public bool IsExhausted => RemainingQuantity == 0;

        public decimal GetTotalCost() => InitialQuantity * UnitCost;

        Guid IMultiTenantEntity.BrandId
        {
            get => BrandId.Value;
            set => BrandId = new BrandId(value);
        }
    }
}
