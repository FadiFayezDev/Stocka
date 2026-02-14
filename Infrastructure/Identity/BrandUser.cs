using Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Identity
{
    public class BrandUser
    {
        public Guid BrandId { get; set; }
        public virtual Brand Brand { get; set; } = null!;

        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; } = null!;

        public string Role { get; set; } = null!;// Manager, Cashier, Owner, etc.
    }
}