using Application.Dtos.Products;
using Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.QueryRepositories
{
    public interface IProductCategoryQueryRepository
    {
        Task<ProductCategoryDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<ProductCategoryDto>> GetAllTableAsync();
        Task<IEnumerable<ProductCategoryDto>> GetAllByBrandIdAsync(Guid brandId);
        Task<ProductCategoryIncludedBrandDto?> GetProductCategoryByIdWithBrandsAsync(Guid id);
        Task<IEnumerable<ProductCategoryIncludedBrandDto>> GetAllProductCategoriesWithBrandsAsync(Guid brandId);
    }
}
