using Application.Dtos.Core;
using Application.UseCases.EmployeeCases;
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

            CreateMap<HireEmployeeCommand, Employee>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => (Guid?)src.ApplicationUserId));

            CreateMap<UpdateEmployeeProfileCommand, Employee>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => (Guid?)src.ApplicationUserId));
        }
    }
}
