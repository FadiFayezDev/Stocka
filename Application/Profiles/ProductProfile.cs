using Application.Common.Interfaces;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Entities.Products;

namespace Application.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // Entity ? DTO
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImagePath))
                .ReverseMap();
        }
    }

    public class ProductProfileFactory 
    {
        private readonly IStorageService _storageService;
        public ProductProfileFactory(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public ProductProfileFactory()
        {

        }

        public  string? GetToken() =>
             _storageService.GetToken();
    }
}