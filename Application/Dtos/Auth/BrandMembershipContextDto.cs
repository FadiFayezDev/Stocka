using Domain.Enums;

namespace Application.Dtos.Auth
{
    public record BrandMembershipContextDto(
        Guid BrandId,
        BrandRole Role
    );
}
