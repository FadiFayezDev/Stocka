using System;

namespace Identity.Contracts.DTOs
{
    public record RoleDto(
        Guid Id,
        string RoleName
    );
}
