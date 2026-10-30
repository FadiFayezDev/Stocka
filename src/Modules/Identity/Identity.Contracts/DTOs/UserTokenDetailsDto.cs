using System;
using System.Collections.Generic;

namespace Identity.Contracts.DTOs
{
    /// <summary>
    /// Minimal identity payload the token generator needs.
    /// Kept transport-agnostic: this record crosses the module boundary.
    /// </summary>
    public record UserTokenDetailsDto(
        Guid UserId,
        string UserName,
        IEnumerable<string> Roles
    );
}
