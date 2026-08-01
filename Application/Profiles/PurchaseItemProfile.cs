using Application.Dtos.Purchasing;
using Application.Features.Commands.PurchaseItem;
using Application.Features.Commands.PurchaseItem.Create;
using Application.Features.Commands.PurchaseItem.Update;
using AutoMapper;
using Domain.Entities.Purchasing;

namespace Application.Profiles
{
    public class PurchaseItemProfile : Profile
    {
        public PurchaseItemProfile()
        {
            // Entity ? DTO
            CreateMap<PurchaseItem, PurchaseItemDto>().ReverseMap();

            // Command ? Entity
            CreateMap<CreatePurchaseItemCommand, PurchaseItem>();
            CreateMap<UpdatePurchaseItemCommand, PurchaseItem>();
        }
    }
}
