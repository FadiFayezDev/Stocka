using Application.Dtos.Purchasing;
using Application.Features.Commands.Supplier;
using Application.Features.Commands.Supplier.Create;
using Application.Features.Commands.Supplier.Update;
using AutoMapper;
using Domain.Entities.Purchasing;

namespace Application.Profiles
{
    public class SupplierProfile : Profile
    {
        public SupplierProfile()
        {
            // Entity ? DTO
            CreateMap<Supplier, SupplierDto>().ReverseMap();

            // Command ? Entity
            CreateMap<CreateSupplierCommand, Supplier>();
            CreateMap<UpdateSupplierCommand, Supplier>();
        }
    }
}
