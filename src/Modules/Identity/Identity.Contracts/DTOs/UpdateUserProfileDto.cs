using System;

namespace Identity.Contracts.DTOs
{
    /// <summary>
    /// Profile update. Names are person data → they persist on the linked Party;
    /// the email is an identity-user field and stays on the user row.
    /// </summary>
    public record UpdateUserProfileDto(
        Guid Id,
        string FirstName,
        string LastName,
        string Email
    );
}
