using Domain.Bases;
using Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Purchasing
{
    public partial class Supplier : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public Guid BrandId { get; set; }

        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }

        private readonly List<Purchase> _purchases = new();

        public virtual Brand Brand { get; set; } = null!;
        public virtual ICollection<Purchase> Purchases => _purchases.AsReadOnly();

        private Supplier() { }

        public Supplier(Guid brandId, string name, string? phone = null, string? email = null, string? address = null)
        {
            Id = Guid.NewGuid();

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Supplier name cannot be empty.", nameof(name));

            BrandId = brandId;
            Name = name.Trim();
            Phone = phone?.Trim();
            Email = email?.Trim();
            Address = address?.Trim();

            ValidateEmail(Email);
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Supplier name cannot be empty.", nameof(newName));

            Name = newName.Trim();
        }

        public void UpdateContactInfo(string? phone, string? email, string? address)
        {
            ValidateEmail(email);

            Phone = phone?.Trim();
            Email = email?.Trim();
            Address = address?.Trim();
        }

        public void AddPurchase(Purchase purchase)
        {
            if (purchase == null)
                throw new ArgumentNullException(nameof(purchase));

            if (purchase.SupplierId != Id)
                throw new ArgumentException("Purchase does not belong to this supplier.");

            if (_purchases.Any(p => p.Id == purchase.Id))
                throw new InvalidOperationException("Purchase already added.");

            _purchases.Add(purchase);
        }

        private static void ValidateEmail(string? email)
        {
            if (!string.IsNullOrWhiteSpace(email) && !email.Contains("@"))
                throw new ArgumentException("Invalid email format.", nameof(email));
        }

        public Guid GetKey() => Id;

        public void SetKey(Guid key) => Id = key;
    }
}