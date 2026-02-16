using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ValueObjects
{
    public class BrandMembership
    {
        public Guid BrandId { get; private set; }
        public Guid UserId { get; private set; }
        public string Role { get; private set; }

        private BrandMembership() { } // 👑 مهم لـ EF

        public BrandMembership(Guid brandId, Guid userId, string role)
        {
            BrandId = brandId;
            UserId = userId;
            Role = role;
        }
    }
}