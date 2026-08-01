using Application.Dtos.Products;
using Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.QueryRepositories
{
    public interface IWarehouseQueryRepository
    {
        Task<WarehouseDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<WarehouseDto>> GetAllTableAsync();
        Task<IEnumerable<WarehouseDto>> GetAllByBrandIdAsync(Guid brandId);
    }
}