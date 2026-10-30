using System;
using System.Collections.Generic;

namespace Identity.Contracts.DTOs
{
    /// <summary>
    /// A Party as stored in the Parties supertype table (person data + organization memberships).
    /// A party's roles are known from the Customers/Employees/Suppliers tables sharing the same Id.
    /// </summary>
    public record PartyDto(
        Guid Id,
        string FirstName,
        string LastName,
        string PhoneNumber,
        IReadOnlyList<Guid> OrganizationIds
    );
}
