using Application.Dtos.Core;
using Application.UseCases.CustomerCases;
using AutoMapper;
using Domain.Entities.Core;

namespace Application.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            // Entity ? DTO
            CreateMap<Customer, CustomerDto>().ReverseMap();

            // Command ? Entity
            CreateMap<RegisterCustomerCommand, Customer>();
            CreateMap<UpdateCustomerProfileCommand, Customer>();
        }
    }
}
