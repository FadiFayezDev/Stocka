using Application.Dtos.Core;
using Application.UseCases.Commands.Customer.Create;
using Application.UseCases.Commands.Customer.Update;
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
            CreateMap<CreateCustomerCommand, Customer>();
            CreateMap<UpdateCustomerCommand, Customer>();
        }
    }
}
