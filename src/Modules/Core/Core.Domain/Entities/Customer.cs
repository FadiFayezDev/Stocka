using BuildingBlocks.Application.Primitives.Identifiers;
using BuildingBlocks.Domain.Bases;
using System.Diagnostics.CodeAnalysis;

namespace Core.Domain.Entities
{
    public class Customer : AggregateRoot<CustomerId>/*, IMultiTenantEntity*/
    {
        public Guid? UserId { get; private set; }
        public BrandId BrandId { get; private set; }
        public int LoyaltyPoints { get; private set; }

        // Make customer independent (store basic contact info like Supplier)
        public string? Name { get; private set; }
        public string? Phone { get; private set; }
        public string? Email { get; private set; }
        public string? Address { get; private set; }

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

        // New constructor: create a standalone customer with contact info (like Supplier)
        [SetsRequiredMembers]
        public Customer(BrandId brandId, string name, string? phone = null, string? email = null, string? address = null, int initialLoyaltyPoints = 0)
        {
            Id = CustomerId.New();

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Customer name cannot be empty.", nameof(name));

            if (initialLoyaltyPoints < 0)
                throw new ArgumentException("Loyalty points cannot be negative.", nameof(initialLoyaltyPoints));

            BrandId = brandId;
            Name = name.Trim();
            Phone = phone?.Trim();
            Email = email?.Trim();
            Address = address?.Trim();
            LoyaltyPoints = initialLoyaltyPoints;

            ValidateEmail(Email);
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

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Customer name cannot be empty.", nameof(newName));

            Name = newName.Trim();
        }

        public void UpdateContactInfo(string? phone, string? email, string? address)
        {
            ValidateEmail(email);

            Phone = phone?.Trim();
            Email = email?.Trim();
            Address = address?.Trim();
        }

        private static void ValidateEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email)) return;
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                if (addr.Address != email.Trim())
                    throw new ArgumentException("Invalid email format.", nameof(email));
            }
            catch (FormatException)
            {
                throw new ArgumentException("Invalid email format.", nameof(email));
            }
        }

        //Guid IMultiTenantEntity.BrandId
        //{
        //    get => BrandId.Value;
        //    set => BrandId = new BrandId(value);
        //}
    }
}
