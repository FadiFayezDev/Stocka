using Application.Dtos.Sales;
using Application.Features.Commands.Sale;
using Application.Features.Commands.Sale.Create;
using Application.Features.Commands.Sale.Update;
using AutoMapper;
using Domain.Entities.Sales;
using Domain.Enums;

namespace Application.Profiles
{
    public class SaleProfile : Profile
    {
        public SaleProfile()
        {
            // Entity ? DTO (with enum conversion)
            CreateMap<Sale, SaleDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<SaleStatus>(src.Status)));

            // Command ? Entity
            CreateMap<CreateSaleCommand, Sale>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<SaleStatus>(src.Status)));

            CreateMap<UpdateSaleCommand, Sale>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<SaleStatus>(src.Status)));
        }
    }
}
