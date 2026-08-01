using Application.Dtos.Products;
using Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.QueryRepositories
{
    public interface IWarehouseBatchQueryRepository
    {
        Task<WarehouseBatchDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<WarehouseBatchDto>> GetAllTableAsync();
        Task<IEnumerable<WarehouseBatchDto>> GetAllByBrandIdAsync(Guid brandId);
    }
}
