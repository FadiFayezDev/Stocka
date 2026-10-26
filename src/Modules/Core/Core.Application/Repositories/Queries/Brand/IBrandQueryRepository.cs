using BuildingBlocks.Application.Primitives.Identifiers;
using Core.Application.DTOs.Brand;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Repositories.Queries.Brand
{
    public interface IBrandQueryRepository
    {
        Task<BrandDto?> GetBrandByIdAsync(BrandId id);
    }
}
