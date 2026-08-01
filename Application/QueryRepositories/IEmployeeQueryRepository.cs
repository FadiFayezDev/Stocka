using Application.Dtos.Core;
using Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.QueryRepositories
{
    public interface IEmployeeQueryRepository
    {
        Task<EmployeeDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<EmployeeDto>> GetAllTableAsync();
        Task<IEnumerable<EmployeeDto>> GetAllByBrandIdAsync(Guid brandId);
    }
}
