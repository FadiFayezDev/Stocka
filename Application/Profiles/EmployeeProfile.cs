using Application.Dtos.Core;
using Application.UseCases.Commands.Employee.Create;
using Application.UseCases.Commands.Employee.Update;
using AutoMapper;
using Domain.Entities.Core;

namespace Application.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            // Entity ? DTO
            CreateMap<Employee, EmployeeDto>().ReverseMap();

            // Command ? Entity
            CreateMap<CreateEmployeeCommand, Employee>();
            CreateMap<UpdateEmployeeCommand, Employee>();
        }
    }
}
