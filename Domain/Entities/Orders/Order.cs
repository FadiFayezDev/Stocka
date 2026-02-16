using Domain.Bases;
using Domain.Entities.Core;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Net.ServerSentEvents;
using System.Text;

namespace Domain.Entities.Orders
{
    public partial class Order : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public Guid BrandId { get; set; }

        public Guid EmployeeId { get; set; }

        public Guid? CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        public OrderStatus Status { get; private set; }

        public decimal TotalAmount { get; private set; }

        private readonly List<OrderItem> _orderItems = new();

        public virtual Brand Brand { get; set; } = null!;

        public virtual Employee? Employee { get; set; }

        public virtual Customer? Customer { get; set; }

        public virtual ICollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        private Order() { }

        public Order(Guid brandId, Guid employeeId, Guid? customerId = null, DateTime? orderDate = null)
        {
            BrandId = brandId;
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

        public void RemoveOrderItem(Guid itemId)
        {
            if (Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("Cannot remove items from a cancelled order.");

            var item = _orderItems.FirstOrDefault(si => si.Id == itemId);
            if (item == null)
                throw new ArgumentException("Order item not found.");

            _orderItems.Remove(item);
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

        private void RecalculateTotal()
        {
            TotalAmount = _orderItems.Sum(si => si.Quantity * si.UnitPrice);
        }

        public Guid GetKey() => Id;

        public void SetKey(Guid key) => Id = key;
    }
}