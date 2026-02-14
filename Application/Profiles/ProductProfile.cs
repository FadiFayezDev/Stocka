using Application.Dtos.Products;
using Application.Features.Commands.Product;
using Application.Features.Commands.Product.Create;
using Application.Features.Commands.Product.Update;
using AutoMapper;
using Domain.Entities.Products;

namespace Application.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // Entity ? DTO
            CreateMap<Product, ProductDto>().ReverseMap();

            // Command ? Entity
            CreateMap<CreateProductCommand, Product>();
            CreateMap<UpdateProductCommand, Product>();
        }
    }
}
