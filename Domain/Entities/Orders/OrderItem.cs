using Domain.Bases;
using Domain.Entities.Core;
using Domain.Entities.Products;
using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Domain.Entities.Orders
{
    public partial class OrderItem : AggregateRoot<OrderItemId>
    {
        public OrderId OrderId { get; private set; }

        public ProductId ProductId { get; private set; }

        public BatchId BatchId { get; private set; }


        public int Quantity { get; private set; }

        public decimal UnitPrice { get; private set; }

        public decimal CostPrice { get; private set; }

        public virtual Batch Batch { get; private set; } = null!;

        public virtual Product Product { get; private set; } = null!;

        public virtual Order Order { get; private set; } = null!;


        private OrderItem() { }

        [SetsRequiredMembers]
        public OrderItem(OrderId orderId, ProductId productId, BatchId batchId, int quantity, decimal unitPrice, decimal costPrice)
        {
            Id = OrderItemId.New();

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

        public void UpdateCostPrice(decimal newCostPrice)
        {
            if (newCostPrice <= 0)
                throw new ArgumentException("Cost price must be greater than zero.", nameof(newCostPrice));

            if (newCostPrice > UnitPrice)
                throw new ArgumentException("Cost price cannot exceed unit price.", nameof(newCostPrice));

            CostPrice = newCostPrice;
        }

        public void UpdatePricing(decimal newUnitPrice, decimal newCostPrice)
        {
            if (newUnitPrice <= 0)
                throw new ArgumentException("Unit price must be greater than zero.", nameof(newUnitPrice));

            if (newCostPrice <= 0)
                throw new ArgumentException("Cost price must be greater than zero.", nameof(newCostPrice));

            if (newUnitPrice < newCostPrice)
                throw new ArgumentException("Unit price cannot be less than cost price.", nameof(newUnitPrice));

            UnitPrice = newUnitPrice;
            CostPrice = newCostPrice;
        }

        public decimal GetProfit() => (UnitPrice - CostPrice) * Quantity;

        public decimal GetTotalPrice() => UnitPrice * Quantity;
    }
}
