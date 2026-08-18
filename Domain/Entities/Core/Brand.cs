using Domain.Bases;
using Domain.Entities.Accounting;
using Domain.Entities.Expenses;
using Domain.Entities.Orders;
using Domain.Entities.Products;
using Domain.Entities.Purchasing;
using Domain.Primitives;
using Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities.Core
{
    public class Brand : AggregateRoot<BrandId>
    {
        public string Name { get; private set; } = null!;
        public string Slug { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; }

        [SetsRequiredMembers]
        public Brand(string name, string slug)
        {
            Id = BrandId.New();
            Name = name;
            Slug = slug;
            CreatedAt = DateTime.UtcNow;
        }

        private readonly List<BrandMembership> _memberships = new();
        public IReadOnlyCollection<BrandMembership> Memberships => _memberships;

        public void Rename(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Brand name cannot be empty.", nameof(newName));

            Name = newName.Trim();
        }

        public void ChangeSlug(string newSlug)
        {
            if (string.IsNullOrWhiteSpace(newSlug))
                throw new ArgumentException("Brand slug cannot be empty.", nameof(newSlug));

            Slug = newSlug.Trim();
        }

        public void AddMember(Guid userId, Domain.Enums.BrandRole role)
        {
            if (!Enum.IsDefined(typeof(Domain.Enums.BrandRole), role))
                throw new ArgumentException("Invalid brand role.", nameof(role));

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
    }
}
