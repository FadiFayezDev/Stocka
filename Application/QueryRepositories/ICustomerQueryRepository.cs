using Application.Dtos.Core;
using Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.QueryRepositories
{
    public interface ICustomerQueryRepository
    {
        Task<CustomerDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<CustomerDto>> GetAllTableAsync();
        Task<IEnumerable<CustomerDto>> GetAllByBrandIdAsync(Guid brandId);
    }
}
