using Application.Dtos.Purchasing;
using Domain.Entities.Purchasing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.QueryRepositories
{
    public interface ISupplierQueryRepository
    {
        Task<SupplierDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<SupplierDto>> GetAllTableAsync();
        Task<IEnumerable<SupplierDto>> GetAllByBrandIdAsync(Guid brandId);
    }
}
