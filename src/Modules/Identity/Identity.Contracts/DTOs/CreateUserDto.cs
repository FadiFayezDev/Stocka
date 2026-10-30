using System;
using System.Collections.Generic;

namespace Identity.Contracts.DTOs
{
    /// <summary>
    /// Input for creating an identity user together with its linked person row.
    /// <para>
    /// Domain invariants baked into the shape:
    /// <list type="bullet">
    ///   <item><see cref="ApplicationUser"/> is 1:1 with a Party (person row) — so the
    ///   person fields (<see cref="FirstName"/>, <see cref="LastName"/>, <see cref="PhoneNumber"/>)
    ///   are stored on the Party, never duplicated on the user;</item>
    ///   <item>a Party is always born inside an organization — hence
    ///   <see cref="OrganizationId"/> is its first membership.</item>
    /// </list>
    /// </para>
    /// </summary>
    public record CreateUserDto(
        Guid OrganizationId,
        string FirstName,
        string LastName,
        string PhoneNumber,
        string UserName,
        string Email,
        string Password,
        IEnumerable<string> Roles
    );
}
