using Application.Dtos.Purchasing;
using Application.Features.Commands.Purchase;
using Application.Features.Commands.Purchase.Create;
using Application.Features.Commands.Purchase.Update;
using AutoMapper;
using Domain.Entities.Purchasing;

namespace Application.Profiles
{
    public class PurchaseProfile : Profile
    {
        public PurchaseProfile()
        {
            // Entity ? DTO
            CreateMap<Purchase, PurchaseDto>().ReverseMap();

            // Command ? Entity
            CreateMap<CreatePurchaseCommand, Purchase>();
            CreateMap<UpdatePurchaseCommand, Purchase>();
        }
    }
}
