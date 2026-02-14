using Application.Dtos.Purchasing;
using Domain.Entities.Purchasing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.QueryRepositories
{
    public interface IPurchaseItemQueryRepository
    {
        Task<PurchaseItemDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<PurchaseItemDto>> GetAllTableAsync();
        Task<IEnumerable<PurchaseItemDto>> GetAllByBrandIdAsync(Guid brandId);
    }
}