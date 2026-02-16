using Domain.Bases;
using Domain.Entities.Core;
using Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Orders
{
    public partial class OrderItem : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }

        public Guid BatchId { get; set; }

        public Guid BrandId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal CostPrice { get; set; }

        public virtual Batch Batch { get; set; } = null!;

        public virtual Product Product { get; set; } = null!;

        public virtual Order Order { get; set; } = null!;

        public virtual Brand Brand { get; set; } = null!;

        private OrderItem() { }

        public OrderItem(Guid orderId, Guid productId, Guid batchId, Guid brandId, int quantity, decimal unitPrice, decimal costPrice)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

            if (unitPrice <= 0)
                throw new ArgumentException("Unit price must be greater than zero.", nameof(unitPrice));

            if (costPrice <= 0)
                throw new ArgumentException("Cost price must be greater than zero.", nameof(costPrice));

            if (unitPrice < costPrice)
                throw new ArgumentException("Unit price cannot be less than cost price.", nameof(unitPrice));

            OrderId = orderId;
            ProductId = productId;
            BatchId = batchId;
            BrandId = brandId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            CostPrice = costPrice;
        }

        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(newQuantity));

            Quantity = newQuantity;
        }

        public void UpdateUnitPrice(decimal newUnitPrice)
        {
            if (newUnitPrice <= 0)
                throw new ArgumentException("Unit price must be greater than zero.", nameof(newUnitPrice));

            if (newUnitPrice < CostPrice)
                throw new ArgumentException("Unit price cannot be less than cost price.", nameof(newUnitPrice));

            UnitPrice = newUnitPrice;
        }

        public decimal GetProfit() => (UnitPrice - CostPrice) * Quantity;

        public decimal GetTotalPrice() => UnitPrice * Quantity;

        public Guid GetKey() => Id;

        public void SetKey(Guid key) => Id = key;
    }
}