using Application.Dtos.Sales;
using Application.Features.Commands.SaleItem;
using Application.Features.Commands.SaleItem.Create;
using Application.Features.Commands.SaleItem.Update;
using AutoMapper;
using Domain.Entities.Sales;

namespace Application.Profiles
{
    public class SaleItemProfile : Profile
    {
        public SaleItemProfile()
        {
            // Entity ? DTO
            CreateMap<SaleItem, SaleItemDto>().ReverseMap();

            // Command ? Entity
            CreateMap<CreateSaleItemCommand, SaleItem>();
            CreateMap<UpdateSaleItemCommand, SaleItem>();
        }
    }
}
