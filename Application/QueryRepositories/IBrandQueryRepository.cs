using Application.Dtos.Core;
using Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.QueryRepositories
{
    public interface IBrandQueryRepository
    {
        Task<BrandDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<BrandDto>> GetAllTableAsync();
        Task<IEnumerable<BrandDto>> GetAllByBrandIdAsync(Guid brandId);
    }
}
