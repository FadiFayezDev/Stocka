using System;
using System.Collections.Generic;

namespace Identity.Contracts.DTOs
{
    /// <summary>
    /// Full user read model: identity fields from the user row, person fields
    /// joined from the linked Party, organization entries from PartyOrganizations.
    /// </summary>
    public record UserDetailsDto(
        Guid Id,
        IReadOnlyList<Guid> OrganizationIds,
        string FirstName,
        string LastName,
        string UserName,
        string Email,
        IReadOnlyList<string> Roles
    );
}
