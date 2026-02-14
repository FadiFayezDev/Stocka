using Application.Dtos.Products;
using Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.QueryRepositories
{
    public interface IProductQueryRepository
    {
        Task<ProductDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<ProductDto>> GetAllTableAsync();
        Task<IEnumerable<ProductDto>> GetAllByBrandIdAsync(Guid brandId);
    }
}
