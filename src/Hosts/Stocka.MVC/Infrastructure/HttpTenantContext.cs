using BuildingBlocks.Application.Abstractions;
using BuildingBlocks.Application.Primitives.Identifiers;

namespace Stocka.MVC.Infrastructure;

public sealed class HttpTenantContext : ITenantContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly BrandId _fallbackBrandId;

    public HttpTenantContext(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        var fallbackString = configuration["Tenant:DefaultBrandId"];
        if (!string.IsNullOrWhiteSpace(fallbackString) && Guid.TryParse(fallbackString, out var g) && g != Guid.Empty)
            _fallbackBrandId = BrandId.FromGuid(g);
        else
            _fallbackBrandId = BrandId.FromGuid(Guid.Parse("11111111-1111-1111-1111-111111111111"));
    }

    public BrandId BrandId
    {
        get
        {
            var ctx = _httpContextAccessor.HttpContext;
            if (ctx != null)
            {
                // 1) Header X-Brand-Id
                if (ctx.Request.Headers.TryGetValue("X-Brand-Id", out var header) && Guid.TryParse(header.FirstOrDefault(), out var hg) && hg != Guid.Empty)
                    return BrandId.FromGuid(hg);

                // 2) Claim brand_id
                var claim = ctx.User?.FindFirst("brand_id")?.Value ?? ctx.User?.FindFirst("BrandId")?.Value;
                if (Guid.TryParse(claim, out var cg) && cg != Guid.Empty)
                    return BrandId.FromGuid(cg);

                // 3) Items override (useful for testing)
                if (ctx.Items.TryGetValue("BrandId", out var item) && item is Guid gg && gg != Guid.Empty)
                    return BrandId.FromGuid(gg);
                if (ctx.Items.TryGetValue("BrandId", out var item2) && item2 is string s && Guid.TryParse(s, out var sg) && sg != Guid.Empty)
                    return BrandId.FromGuid(sg);
            }

            return _fallbackBrandId;
        }
    }

    public Guid? UserId
    {
        get
        {
            var ctx = _httpContextAccessor.HttpContext;
            var uid = ctx?.User?.FindFirst("sub")?.Value ?? ctx?.User?.FindFirst("UserId")?.Value;
            if (Guid.TryParse(uid, out var g)) return g;
            return null;
        }
    }
}
