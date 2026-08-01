using Application.Dtos.Orders;
using Application.Features.Commands.OrderItem;
using Application.Features.Commands.OrderItem.Create;
using Application.Features.Commands.OrderItem.Update;
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

            // Command ? Entity
            CreateMap<CreateOrderItemCommand, OrderItem>();
            CreateMap<UpdateOrderItemCommand, OrderItem>();
        }
    }
}
