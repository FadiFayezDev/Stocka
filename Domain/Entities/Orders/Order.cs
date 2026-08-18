using Domain.Bases;
using Domain.Entities.Core;
using Domain.Entities.Products;
using Domain.Enums;
using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.ServerSentEvents;
using System.Text;

namespace Domain.Entities.Orders
{
    public partial class Order : AggregateRoot<OrderId>, IMultiTenantEntity, IBranchScopedEntity
    {
        public BrandId BrandId { get; private set; }
        public BranchId? BranchId { get; private set; }

        public EmployeeId EmployeeId { get; private set; }

        public CustomerId? CustomerId { get; private set; }

        public DateTime OrderDate { get; private set; }

        public OrderStatus Status { get; private set; }

        public decimal TotalAmount { get; private set; }

        private readonly List<OrderItem> _orderItems = new();

        public virtual ICollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        private Order() { }

        [SetsRequiredMembers]
        public Order(BrandId brandId, EmployeeId employeeId, CustomerId? customerId = null, DateTime? orderDate = null, BranchId? branchId = null)
        {
            Id = OrderId.New();

            BrandId = brandId;
            BranchId = branchId;
            EmployeeId = employeeId;
            CustomerId = customerId;
            OrderDate = orderDate ?? DateTime.UtcNow;
            Status = OrderStatus.Completed;
            TotalAmount = 0;
        }

        public void AddOrderItem(OrderItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("Cannot add items to a cancelled order.");

            if (item.OrderId != Id)
                throw new ArgumentException("Order item does not belong to this order.");

            if (_orderItems.Any(si => si.Id == item.Id))
                throw new InvalidOperationException("Order item already added.");

            _orderItems.Add(item);
            RecalculateTotal();
        }

        public OrderItem AddOrderItem(ProductId productId, BatchId batchId, int quantity, decimal unitPrice, decimal costPrice)
        {
            var item = new OrderItem(Id, productId, batchId, quantity, unitPrice, costPrice);
            AddOrderItem(item);
            return item;
        }

        public void RemoveOrderItem(OrderItemId itemId)
        {
            if (Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("Cannot remove items from a cancelled order.");

            var item = _orderItems.FirstOrDefault(si => si.Id == itemId);
            if (item == null)
                throw new ArgumentException("Order item not found.");

            _orderItems.Remove(item);
            RecalculateTotal();
        }

        public void UpdateOrderItem(OrderItemId itemId, int quantity, decimal unitPrice, decimal costPrice)
        {
            if (Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("Cannot update items of a cancelled order.");

            var item = _orderItems.FirstOrDefault(si => si.Id == itemId);
            if (item == null)
                throw new ArgumentException("Order item not found.");

            item.UpdateQuantity(quantity);
            item.UpdatePricing(unitPrice, costPrice);
            RecalculateTotal();
        }

        public void CancelOrder()
        {
            if (Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("Order is already cancelled.");

            Status = OrderStatus.Cancelled;
        }

        public void ReturnOrder()
        {
            if (Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("Cannot return a cancelled order.");

            Status = OrderStatus.Returned;
        }

        public void UpdateOrderDate(DateTime newDate)
        {
            if (newDate > DateTime.UtcNow)
                throw new ArgumentException("Order date cannot be in the future.", nameof(newDate));

            OrderDate = newDate;
        }

        public void UpdateStatus(OrderStatus newStatus)
        {
            if (Status == OrderStatus.Cancelled && newStatus != OrderStatus.Cancelled)
                throw new InvalidOperationException("Cannot change status of a cancelled order.");

            if (newStatus == Status)
                throw new InvalidOperationException("Order is already in this status.");

            Status = newStatus;
        }

        private void RecalculateTotal()
        {
            TotalAmount = _orderItems.Sum(si => si.Quantity * si.UnitPrice);
        }

        Guid IMultiTenantEntity.BrandId
        {
            get => BrandId.Value;
            set => BrandId = new BrandId(value);
        }

        Guid IBranchScopedEntity.BranchId
        {
            get => BranchId?.Value ?? Guid.Empty;
            set => BranchId = value == Guid.Empty ? null : new BranchId(value);
        }
    }
}
