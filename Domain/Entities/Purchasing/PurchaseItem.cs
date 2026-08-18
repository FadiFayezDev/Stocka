using Domain.Bases;
using Domain.Entities.Products;
using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Domain.Entities.Purchasing
{
    public partial class PurchaseItem : Entity<PurchaseItemId>
    {
        public PurchaseId PurchaseId { get; private set; }

        public ProductId ProductId { get; private set; }

        public int Quantity { get; private set; }

        public decimal UnitCost { get; private set; }

        public virtual Purchase Purchase { get; private set; } = null!;

        [NotMapped]
        public decimal TotalCost => Quantity * UnitCost;

        private PurchaseItem() { }

        [SetsRequiredMembers]
        public PurchaseItem(PurchaseId purchaseId, ProductId productId, int quantity, decimal unitCost)
        {
            Id = PurchaseItemId.New();

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

            if (unitCost <= 0)
                throw new ArgumentException("Unit cost must be greater than zero.", nameof(unitCost));

            PurchaseId = purchaseId;
            ProductId = productId;
            Quantity = quantity;
            UnitCost = unitCost;
        }

        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(newQuantity));

            Quantity = newQuantity;
        }

        public void UpdateUnitCost(decimal newUnitCost)
        {
            if (newUnitCost <= 0)
                throw new ArgumentException("Unit cost must be greater than zero.", nameof(newUnitCost));

            UnitCost = newUnitCost;
        }
    }
}
