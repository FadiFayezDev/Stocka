using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Auth
{
    public record UserTokenDetailsDto(
        Guid UserId,
        List<Guid> BrandIds,
        string UserName,
        IList<string> Roles
    );
}
