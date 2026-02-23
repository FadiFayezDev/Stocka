using Application.Dtos.Products;

namespace Application.QueryRepositories
{
    public interface IProductQueryRepository
    {
        Task<ProductDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<ProductDto>> GetAllTableAsync();
        Task<IEnumerable<ProductDto>> GetAllByBrandIdAsync(Guid brandId);
        Task<ProductDto?> GetProductsWithQuantityAsync(Guid productId);
        Task<IEnumerable<ProductDto>> GetProductsWithQuantities(Guid brandId);
    }
}