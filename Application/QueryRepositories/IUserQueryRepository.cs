using Application.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.QueryRepositories
{
    public interface IUserQueryRepository
    {
        Task<UserSummaryDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<UserSummaryDto>> GetAllTableAsync();
        Task<IEnumerable<UserSummaryDto>> GetAllByBrandIdAsync(Guid brandId);
    }
}
