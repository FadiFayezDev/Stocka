using Application.Dtos.Products;
using Application.Features.Commands.ProductCategory;
using Application.Features.Commands.ProductCategory.Create;
using Application.Features.Commands.ProductCategory.Update;
using AutoMapper;
using Domain.Entities.Products;

namespace Application.Profiles
{
    public class ProductCategoryProfile : Profile
    {
        public ProductCategoryProfile()
        {
            // Entity ? DTOs
            CreateMap<ProductCategory, ProductCategoryDto>().ReverseMap();
            CreateMap<ProductCategory, ProductCategoryIncludedBrandDto>().ReverseMap();

            // Command ? Entity
            CreateMap<CreateProductCategoryCommand, ProductCategory>();
            CreateMap<UpdateProductCategoryCommand, ProductCategory>();
        }
    }
}
