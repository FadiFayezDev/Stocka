using Application.Dtos.Orders;
using AutoMapper;
using Domain.Entities.Orders;

namespace Application.Profiles
{
    public class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            // Entity ? DTO
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
        }
    }
}
