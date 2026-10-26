using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.DTOs.Category 
{ 
    public record CategoryDto(
        Guid Id,
        Guid BrandId,
        string Name,
        bool IsActive
    );
}