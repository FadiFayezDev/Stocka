using BuildingBlocks.Application.Primitives.Identifiers;

namespace BuildingBlocks.Application.Abstractions;

/// <summary>
/// Fallback tenant context used when no HTTP context is available (e.g., background jobs, tests).
/// Returns a deterministic development BrandId so that create + list use the same tenant.
/// Configure via "Tenant:DefaultBrandId" in appsettings.json.
/// </summary>
public sealed class DefaultTenantContext : ITenantContext
{
    private readonly BrandId _brandId;

    public DefaultTenantContext(BrandId brandId)
    {
        _brandId = brandId;
    }

    public DefaultTenantContext(string? brandIdString = null)
    {
        if (!string.IsNullOrWhiteSpace(brandIdString) && Guid.TryParse(brandIdString, out var guid) && guid != Guid.Empty)
            _brandId = BrandId.FromGuid(guid);
        else
            // Deterministic dev fallback — same across restarts so data remains queryable
            _brandId = BrandId.FromGuid(Guid.Parse("11111111-1111-1111-1111-111111111111"));
    }

    public BrandId BrandId => _brandId;
    public Guid? UserId => null;
}
