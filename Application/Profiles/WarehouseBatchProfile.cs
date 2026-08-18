using Application.Dtos.Products;
using AutoMapper;
using Domain.Entities.Products;

namespace Application.Profiles
{
    public class WarehouseBatchProfile : Profile
    {
        public WarehouseBatchProfile()
        {
            // Entity ? DTO
            CreateMap<WarehouseBatch, WarehouseBatchDto>().ReverseMap();
        }
    }
}
