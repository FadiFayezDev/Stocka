using Domain.Bases;
using Domain.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Core
{
    // Domain/Entities/Customer.cs
    public class Customer : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public void SetKey(Guid key) => Id = key;

        public Guid? UserId { get; set; }
        public Guid? BrandId { get; set; } // optional for marketplace scenarios
        public int LoyaltyPoints { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

        public Guid GetKey()
        {
            return Id;
        }
    }
}