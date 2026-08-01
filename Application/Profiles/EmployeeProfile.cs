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
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom(src => src.UserId))
                .ReverseMap()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.ApplicationUserId == Guid.Empty ? (Guid?)null : src.ApplicationUserId));

            CreateMap<CreateEmployeeCommand, Employee>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => (Guid?)src.ApplicationUserId));

            CreateMap<UpdateEmployeeCommand, Employee>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => (Guid?)src.ApplicationUserId));
        }
    }
}
