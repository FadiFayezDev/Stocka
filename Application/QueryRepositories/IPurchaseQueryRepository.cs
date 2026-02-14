using Application.Dtos.Purchasing;
using Domain.Entities.Purchasing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.QueryRepositories
{
    public interface IPurchaseQueryRepository
    {
        Task<PurchaseDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<PurchaseDto>> GetAllTableAsync();
        Task<IEnumerable<PurchaseDto>> GetAllByBrandIdAsync(Guid brandId);
    }
}
