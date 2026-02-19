using Domain.Bases;
using Domain.Entities.Accounting;
using Domain.Entities.Expenses;
using Domain.Entities.Products;
using Domain.Entities.Purchasing;
using Domain.Entities.Orders;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Domain.Entities.Core
{
    public class Brand : IEntity<Guid>
    {
        public Guid Id { get; private set; }

        public void SetKey(Guid key) => Id = key;

        public string Name { get; private set; } = null!;
        public string Slug { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; }

        public Brand(string name, string slug)
        {
            Id = Guid.NewGuid();
            Name = name;
            Slug = slug;
            CreatedAt = DateTime.UtcNow;
        }

        private readonly List<BrandMembership> _memberships = new();
        public IReadOnlyCollection<BrandMembership> Memberships => _memberships;

        private readonly List<Branch> _branches = new();
        public IReadOnlyCollection<Branch> Branches => _branches.AsReadOnly();



        // Navigation Properties
        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
        public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
        public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
        public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
        public virtual ICollection<ExpenseCategory> ExpenseCategories { get; set; } = new List<ExpenseCategory>();
        public virtual ICollection<JournalEntry> JournalEntries { get; set; } = new List<JournalEntry>();

        public void AddMember(Guid userId, string role)
        {
            if (_memberships.Any(m => m.UserId == userId))
                throw new ArgumentException("User already a member");

            _memberships.Add(new BrandMembership(Id, userId, role));
        }

        public void RemoveMember(Guid userId)
        {
            var membership = _memberships
                .FirstOrDefault(m => m.UserId == userId);

            if (membership == null)
                throw new ArgumentException("User not a member");

            _memberships.Remove(membership);
        }

        public void AddBranch(Branch branch)
        {
            if (branch == null)
                throw new ArgumentNullException(nameof(branch));

            if (branch.BrandId != Id)
                throw new ArgumentException("Branch does not belong to this brand.");

            if (_branches.Any(b => b.Id == branch.Id))
                throw new InvalidOperationException("Branch already exists in this brand.");

            _branches.Add(branch);
        }

        public void RemoveBranch(Guid branchId)
        {
            var branch = _branches.FirstOrDefault(b => b.Id == branchId);
            if (branch == null)
                throw new ArgumentException("Branch not found in this brand.");

            _branches.Remove(branch);
        }

        public Guid GetKey() => Id;
    }
}