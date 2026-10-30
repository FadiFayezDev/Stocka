using BuildingBlocks.Application.Common.Interfaces;
using BuildingBlocks.Domain.Enums;
using Identity.Infrastructure.Authentication;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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

        public Guid ActiveOrganizationId
        {
            get
            {
                var val =
                    User?.FindFirstValue(ClaimNames.ActiveOrganizationId) ??
                    User?.FindFirstValue("Organization") ??
                    User?.FindFirstValue("OrganizationId");

                return Guid.TryParse(val, out var OrganizationId) ? OrganizationId : Guid.Empty;
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

        public OrganizationRole Role
        {
            get
            {
                var val = User?.FindFirstValue(ClaimNames.OrganizationRole) ?? User?.FindFirstValue("organization_role");
                if (val == null)
                    throw new ArgumentException("Organization role is not found.");
                Enum.TryParse<OrganizationRole>(val, out var role);
                return role;
            }
        }

        public bool IsOwner => Role == OrganizationRole.Owner;

        public bool CanAccessAllBranches => Role is OrganizationRole.Owner or OrganizationRole.OrganizationAdmin;
    }
}
