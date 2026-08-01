using Application.Dtos.Orders;
using Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.QueryRepositories
{
    public interface IOrderItemQueryRepository
    {
        Task<OrderItemDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<OrderItemDto>> GetAllTableAsync();
        Task<IEnumerable<OrderItemDto>> GetAllByBrandIdAsync(Guid brandId);
    }
}
