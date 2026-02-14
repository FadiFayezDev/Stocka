using Application.Dtos.Sales;
using Domain.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.QueryRepositories
{
    public interface ISaleQueryRepository
    {
        Task<SaleDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<SaleDto>> GetAllTableAsync();
        Task<IEnumerable<SaleDto>> GetAllByBrandIdAsync(Guid brandId);
    }
}
