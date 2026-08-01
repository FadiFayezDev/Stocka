using System;
using Domain.Bases;
using Domain.Enums;
using Domain.Primitives;

namespace Domain.ValueObjects
{
    public class BrandMembership : IMultiTenantEntity
    {
        public BrandId BrandId { get; private set; }
        public Guid UserId { get; private set; }
        public BrandRole Role { get; private set; }

        private BrandMembership() { } // 👑 مهم لـ EF

        public BrandMembership(BrandId brandId, Guid userId, BrandRole role)
        {
            BrandId = brandId;
            UserId = userId;
            Role = role;
        }

        Guid IMultiTenantEntity.BrandId 
        { 
            get => BrandId.Value; 
            set => BrandId = new BrandId(value); 
        }
    }
}