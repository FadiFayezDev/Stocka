using Application.Dtos.Products;
using Application.UseCases.Category;
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
            CreateMap<ProductCategoryDto, ProductCategory>().ReverseMap();
            CreateMap<ProductCategory, ProductCategoryIncludedBrandDto>().ReverseMap();

            // Command ? Entity
            CreateMap<RegisterProductCategoryCommand, ProductCategory>();
            CreateMap<UpdateProductCategoryCommand, ProductCategory>();
        }
    }
}
