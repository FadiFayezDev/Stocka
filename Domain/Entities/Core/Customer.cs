using Domain.Bases;
using Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities.Core
{
    // Domain/Entities/Customer.cs
    public class Customer : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? BrandId { get; set; }
        public int LoyaltyPoints { get; set; }

        private readonly List<Order> _Orders = new();
        public virtual Brand Brand { get; set; }
        public virtual ICollection<Order> Orders => _Orders.AsReadOnly();

        public Customer() { }

        public Customer(Guid? userId, Guid? brandId, int initialLoyaltyPoints = 0)
        {
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

        public void AddOrder(Order Order)
        {
            if (Order == null)
                throw new ArgumentNullException(nameof(Order));

            if (BrandId.HasValue && Order.BrandId != BrandId)
                throw new ArgumentException("Order does not belong to this customer's brand.");

            if (_Orders.Any(s => s.Id == Order.Id))
                throw new InvalidOperationException("Order already added.");

            _Orders.Add(Order);
        }

        public Guid GetKey()
        {
            return Id;
        }

        public void SetKey(Guid key) => Id = key;
    }
}