using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.DTOs.Product
{
    public record ProductDto(
        Guid Id,
        Guid BrandId,
        string Name,
        string Barcode,
        Guid CategoryId,
        decimal Price,
        bool IsActive,
        string ImagePath
    );
}