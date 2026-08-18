using Domain.Bases;
using Domain.Entities.Core;
using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Domain.Entities.Purchasing
{
    public partial class Supplier : AggregateRoot<SupplierId>, IMultiTenantEntity
    {
        public BrandId BrandId { get; private set; }

        public string Name { get; private set; } = null!;
        public string? Phone { get; private set; }
        public string? Email { get; private set; }
        public string? Address { get; private set; }

        private Supplier() { }

        [SetsRequiredMembers]
        public Supplier(BrandId brandId, string name, string? phone = null, string? email = null, string? address = null)
        {
            Id = SupplierId.New();

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

        private static void ValidateEmail(string? email)
        {
            if (!string.IsNullOrWhiteSpace(email) && !email.Contains("@"))
                throw new ArgumentException("Invalid email format.", nameof(email));
        }

        Guid IMultiTenantEntity.BrandId
        {
            get => BrandId.Value;
            set => BrandId = new BrandId(value);
        }
    }
}
