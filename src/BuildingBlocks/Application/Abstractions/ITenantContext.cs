using BuildingBlocks.Application.Primitives.Identifiers;

namespace BuildingBlocks.Application.Abstractions;

public interface ITenantContext
{
    BrandId BrandId { get; }
    Guid? UserId { get; }
}
