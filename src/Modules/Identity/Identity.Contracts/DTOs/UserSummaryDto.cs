using System;

namespace Identity.Contracts.DTOs
{
    public record UserSummaryDto(
        Guid Id,
        string FirstName,
        string LastName,
        string UserName,
        string Email
    );
}
