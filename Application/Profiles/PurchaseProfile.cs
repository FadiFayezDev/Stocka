using Application.Dtos.Purchasing;
using Application.UseCases.Purchase;
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
            CreateMap<ReceivePurchaseCommand, Purchase>();
            CreateMap<UpdateReceivedPurchaseCommand, Purchase>();
        }
    }
}
