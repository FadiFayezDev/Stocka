using Application.Dtos.Orders;
using Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.QueryRepositories
{
    public interface IOrderQueryRepository
    {
        Task<OrderDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<OrderDto>> GetAllTableAsync();
        Task<IEnumerable<OrderDto>> GetAllByBrandIdAsync(Guid brandId);
        Task<OrderWithItemsDto?> GetByIdWithItemsAsync(Guid orderId);
        Task<IEnumerable<OrderWithItemsDto>> GetAllWithItemsAsync();
        Task<IEnumerable<OrderWithItemsDto>> GetAllWithItemsByBrandIdAsync(Guid brandId);
    }
}
