using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Auth
{

    // 6. DTO للتعامل مع الـ Roles
    public record RoleDto(
        Guid Id,
        string RoleName
    );
}
