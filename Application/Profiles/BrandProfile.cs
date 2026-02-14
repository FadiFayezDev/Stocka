using Application.Dtos.Core;
using Application.UseCases.Commands.Brand.Create;
using Application.UseCases.Commands.Brand.Update;
using AutoMapper;
using Domain.Entities.Core;

namespace Application.Profiles
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            // Entity ? DTO
            CreateMap<Brand, BrandDto>().ReverseMap();

            // Command ? Entity
            CreateMap<CreateBrandCommand, Brand>();
            CreateMap<UpdateBrandCommand, Brand>();
        }
    }
}
