using Application.Dtos.Orders;
using AutoMapper;
using Domain.Entities.Orders;
using Domain.Enums;

namespace Application.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<OrderStatus>(src.Status)));
        }
    }
}
