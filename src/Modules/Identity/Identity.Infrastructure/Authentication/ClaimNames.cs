namespace Identity.Infrastructure.Authentication
{
    /// <summary>Custom claim type names written by the module's token generator.</summary>
    public static class ClaimNames
    {
        /// <summary>ApplicationUser.Id — lets bearer consumers resolve the identity row.</summary>
        public const string UserId = "user_id";

        /// <summary>Raw JWT carried by the MVC token cookie (claim → used later for API calls).</summary>
        public const string Token = "token";
        public const string ActiveOrganizationId = "active_organization_id";
        public const string ActiveBranchId = "active_branch_id";
        public const string OrganizationRole = "organization_role";
        public const string organizationAccess = "organization_access";
    }
}
