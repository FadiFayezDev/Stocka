using Domain.Bases;
using Domain.Entities.Orders;
using Domain.Primitives;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities.Core
{
    public class Customer : AggregateRoot<CustomerId>, IMultiTenantEntity
    {
        public Guid? UserId { get; private set; }
        public BrandId BrandId { get; private set; }
        public int LoyaltyPoints { get; private set; }

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

        Guid IMultiTenantEntity.BrandId
        {
            get => BrandId.Value;
            set => BrandId = new BrandId(value);
        }
    }
}
