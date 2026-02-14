using Application.Dtos.Products;
using Application.Features.Commands.Warehouse;
using AutoMapper;
using Domain.Entities.Products;
using Domain.Enums;

namespace Application.Profiles
{
    public class WarehouseProfile : Profile
    {
        public WarehouseProfile()
        {
            // Entity ? DTO (with enum conversion)
            CreateMap<Warehouse, WarehouseDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<WarehouseType>(src.Type)));
        }
    }
}
