using BuildingBlocks.Application.Primitives.Identifiers;
using BuildingBlocks.Domain.Bases;
using Core.Domain.Enums;

namespace Core.Domain.Primitives.ValueObjects
{
    public class BrandMembership : ValueObject /*: IMultiTenantEntity*/
    {
        public BrandId BrandId { get; private set; }
        public Guid UserId { get; private set; }
        public BrandRole Role { get; private set; }

        private BrandMembership() { } 

        public BrandMembership(BrandId brandId, Guid userId, BrandRole role)
        {
            BrandId = brandId;
            UserId = userId;
            Role = role;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return BrandId;
            yield return UserId;
            yield return Role;
        }

        //Guid IMultiTenantEntity.BrandId 
        //{ 
        //    get => BrandId.Value; 
        //    set => BrandId = new BrandId(value); 
        //}
    }
}