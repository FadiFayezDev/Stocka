using Domain.Enums;

namespace Application.Dtos.Auth
{
    public record UserTokenDetailsDto(
        Guid UserId,
        string UserName,
        IList<string> Roles,
        Guid ActiveBrandId,
        BrandRole BrandRole,
        Guid? ActiveBranchId = default
    );
}
