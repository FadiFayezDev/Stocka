using Application.Dtos.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos
{
    public record OnboardBrandOwnerDto(
        string token, 
        BrandDto brand
    );
}