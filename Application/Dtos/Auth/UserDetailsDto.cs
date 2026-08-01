using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Auth
{
    public record UserDetailsDto(
        Guid UserId,
        List<Guid> BrandIds, // One user can be associated with multiple brands
        string FirstName,
        string LastName,
        string UserName,
        string Email,
        IList<string> Roles
    );
}