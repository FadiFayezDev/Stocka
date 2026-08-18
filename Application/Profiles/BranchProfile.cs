using Application.Dtos.Core;
using Application.UseCases.BranchCases;
using AutoMapper;
using Domain.Entities.Core;

namespace Application.Profiles
{
    public class BranchProfile : Profile
    {
        public BranchProfile()
        {
            // Entity ? DTO
            CreateMap<Branch, BranchDto>().ReverseMap();

            // Command ? Entity
            CreateMap<RegisterBranchCommand, Branch>();
        }
    }
}
