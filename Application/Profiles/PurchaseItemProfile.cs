using Application.Dtos.Purchasing;
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
        }
    }
}
