using Application.Dtos.Products;
using Application.UseCases.WarehouseCases;
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
                //.ForMember(dest => dest.BranchIds, opt => opt.MapFrom(src => src.WarehouseBranches.Select(wb => wb.BranchId).ToList()))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.GetName(typeof(WarehouseType), src.Type)))
                .ReverseMap()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<WarehouseType>(src.Type)));

            CreateMap<RegisterWarehouseCommand, Warehouse>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (WarehouseType)src.Type))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}