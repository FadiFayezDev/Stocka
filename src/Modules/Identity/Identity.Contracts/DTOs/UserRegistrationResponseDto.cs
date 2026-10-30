using System;

namespace Identity.Contracts.DTOs
{
    /// <summary>Result of creating a user: both halves of the 1:1 pair.</summary>
    public record UserRegistrationResponseDto(
        bool IsSuccessful,
        Guid UserId,
        Guid PartyId
    );
}
