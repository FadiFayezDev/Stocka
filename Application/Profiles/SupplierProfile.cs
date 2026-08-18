using Application.Dtos.Purchasing;
using Application.UseCases.SupplierCases;
using AutoMapper;
using Domain.Entities.Purchasing;

namespace Application.Profiles
{
    public class SupplierProfile : Profile
    {
        public SupplierProfile()
        {
            // Entity ? DTO
            CreateMap<Supplier, SupplierDto>()
                .ReverseMap();

            // Command ? Entity
            CreateMap<RegisterSupplierCommand, Supplier>();

            CreateMap<UpdateSupplierProfileCommand, Supplier>();
        }
    }
}