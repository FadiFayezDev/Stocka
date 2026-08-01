using Domain.Bases;
using Domain.Entities.Orders;
using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Domain.Entities.Core
{
    public class Customer : AggregateRoot<CustomerId>, IMultiTenantEntity
    {
        public Guid? UserId { get; private set; }
        public BrandId BrandId { get; private set; }
        public int LoyaltyPoints { get; private set; }

        private readonly List<Order> _Orders = new();
        public virtual Brand Brand { get; private set; } = null!;
        public virtual ICollection<Order> Orders => _Orders.AsReadOnly();

        private Customer() { }

        [SetsRequiredMembers]
        public Customer(Guid? userId, BrandId brandId, int initialLoyaltyPoints = 0)
        {
            Id = CustomerId.New();

            if (initialLoyaltyPoints < 0)
                throw new ArgumentException("Loyalty points cannot be negative.", nameof(initialLoyaltyPoints));

            UserId = userId;
            BrandId = brandId;
            LoyaltyPoints = initialLoyaltyPoints;
        }

        public void AddLoyaltyPoints(int points)
        {
            if (points <= 0)
                throw new ArgumentException("Points to add must be greater than zero.", nameof(points));

            LoyaltyPoints += points;
        }

        public void DeductLoyaltyPoints(int points)
        {
            if (points <= 0)
                throw new ArgumentException("Points to deduct must be greater than zero.", nameof(points));

            if (points > LoyaltyPoints)
                throw new InvalidOperationException("Insufficient loyalty points.");

            LoyaltyPoints -= points;
        }

        public void SetLoyaltyPoints(int points)
        {
            if (points < 0)
                throw new ArgumentException("Loyalty points cannot be negative.", nameof(points));

            LoyaltyPoints = points;
        }

        public void AddOrder(Order Order)
        {
            if (Order == null)
                throw new ArgumentNullException(nameof(Order));

            if (Order.BrandId != BrandId)
                throw new ArgumentException("Order does not belong to this customer's brand.");

            if (_Orders.Any(s => s.Id == Order.Id))
                throw new InvalidOperationException("Order already added.");

            _Orders.Add(Order);
        }

        Guid IMultiTenantEntity.BrandId
        {
            get => BrandId.Value;
            set => BrandId = new BrandId(value);
        }
    }
}
