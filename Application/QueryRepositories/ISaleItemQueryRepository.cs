using Application.Dtos.Sales;
using Domain.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.QueryRepositories
{
    public interface ISaleItemQueryRepository
    {
        Task<SaleItemDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<SaleItemDto>> GetAllTableAsync();
        Task<IEnumerable<SaleItemDto>> GetAllByBrandIdAsync(Guid brandId);
    }
}
