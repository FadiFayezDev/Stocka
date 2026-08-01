using Application.Dtos.Products;
using Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.QueryRepositories
{
    public interface IStockMovementQueryRepository
    {
        Task<StockMovementDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<StockMovementDto>> GetAllTableAsync();
        Task<IEnumerable<StockMovementDto>> GetAllByBrandIdAsync(Guid brandId);
    }
}
