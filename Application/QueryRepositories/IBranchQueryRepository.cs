using Application.Dtos.Core;
using Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.QueryRepositories
{
    public interface IBranchQueryRepository
    {
        Task<BranchDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<BranchDto>> GetAllTableAsync();
        Task<IEnumerable<BranchDto>> GetAllByBrandIdAsync(Guid brandId);
    }
}