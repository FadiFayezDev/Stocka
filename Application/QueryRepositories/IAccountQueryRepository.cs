using Application.Dtos.Accounting;
using Domain.Entities.Accounting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.QueryRepositories
{
    public interface IAccountQueryRepository
    {
        Task<AccountDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<AccountDto>> GetAllTableAsync();
        Task<IEnumerable<AccountDto>> GetAllByBrandIdAsync(Guid brandId);
    }
}    