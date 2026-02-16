using Application.Dtos.Orders;
using Application.Features.Commands.Order;
using Application.Features.Commands.Order.Create;
using Application.Features.Commands.Order.Update;
using AutoMapper;
using Domain.Entities.Orders;
using Domain.Enums;

namespace Application.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            // Entity ? DTO (with enum conversion)
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<OrderStatus>(src.Status)));

            // Command ? Entity
            CreateMap<CreateOrderCommand, Order>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<OrderStatus>(src.Status)));

            CreateMap<UpdateOrderCommand, Order>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<OrderStatus>(src.Status)));
        }
    }
}
