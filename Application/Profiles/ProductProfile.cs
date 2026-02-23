using Application.Common.Interfaces;
using Application.Dtos.Products;
using Application.Features.Commands.Product;
using Application.Features.Commands.Product.Create;
using Application.Features.Commands.Product.Update;
using AutoMapper;
using Domain.Entities.Products;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

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

            // Command ? Entity
            CreateMap<CreateProductCommand, Product>().ConstructUsing(src => new Product(
                src.BrandId, 
                src.CategoryId, 
                src.Name, 
                src.SellingPrice, 
                src.Barcode
            ));
            CreateMap<UpdateProductCommand, Product>();
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