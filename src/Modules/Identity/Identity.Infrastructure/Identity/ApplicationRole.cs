using Microsoft.AspNetCore.Identity;

namespace Identity.Infrastructure.Identity
{
    /// <summary>
    /// Module-scoped role type for the EF Identity stores.
    /// Deliberately plain — the role-level person residue (names / PartyId) was removed;
    /// any future role metadata should go through role claims, not extra columns.
    /// </summary>
    public class ApplicationRole : IdentityRole<Guid>
    {
    }
}
