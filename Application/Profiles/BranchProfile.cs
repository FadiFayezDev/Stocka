using Application.Dtos.Core;
using Application.Features.Commands.Branch;
using Application.Features.Commands.Branch.Create;
using Application.Features.Commands.Branch.Update;
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
            CreateMap<CreateBranchCommand, Branch>();
            CreateMap<UpdateBranchCommand, Branch>();
        }
    }
}
