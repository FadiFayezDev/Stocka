using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Contracts.DTOs
{
    public record CustomerDto(
        Guid Id,
        string FirstName,
        string LastName,
        string PhoneNumber,
        int LoyaltyPoint
    );
}
