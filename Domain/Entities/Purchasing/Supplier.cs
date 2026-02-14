using Domain.Bases;
using Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Purchasing
{
    public partial class Supplier : IEntity<Guid>
    {
        public Guid GetKey() => Id;

        public void SetKey(Guid key) => Id = key;

        public Guid Id { get; set; }

        public Guid BrandId { get; set; }

        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }

        public virtual Brand Brand { get; set; } = null!;

        public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
    }
}