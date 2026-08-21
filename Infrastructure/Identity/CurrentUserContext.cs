using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Common.Interfaces;
using Application.Common.Security;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Identity
{
    public class CurrentUserContext : ICurrentUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
        public Guid UserId
        {
            get
            {
                var id = User?.FindFirstValue(JwtRegisteredClaimNames.Sub) ??
                         User?.FindFirstValue(ClaimNames.UserId) ??
                         User?.FindFirstValue("UserId") ??
                         User?.FindFirstValue(ClaimTypes.NameIdentifier);

                return Guid.TryParse(id, out var userId) ? userId : Guid.Empty;
            }
        }

        public Guid ActiveBrandId
        {
            get
            {
                var val =
                    User?.FindFirstValue(ClaimNames.ActiveBrandId) ??
                    User?.FindFirstValue("brand") ??
                    User?.FindFirstValue("brandId");

                return Guid.TryParse(val, out var brandId) ? brandId : Guid.Empty;
            }
        }

        public Guid? ActiveBranchId
        {
            get
            {
                var val =
                    User?.FindFirstValue(ClaimNames.ActiveBranchId) ??
                    User?.FindFirstValue("branch");

                if (Guid.TryParse(val, out var branchId))
                    return branchId;

                return null;
            }
        }

        public BrandRole Role
        {
            get
            {
                var val = User?.FindFirstValue(ClaimNames.BrandRole) ?? User?.FindFirstValue("brand_role");
                if (val == null)
                    throw new ArgumentException("brand role is not found.");
                Enum.TryParse<BrandRole>(val, out var role);
                return role;
            }
        }

        public bool IsOwner => Role == BrandRole.Owner;

        public bool CanAccessAllBranches => Role is BrandRole.Owner or BrandRole.BrandAdmin;
    }
}
