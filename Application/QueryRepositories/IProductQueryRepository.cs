using Application.Dtos.Products;
using Application.Dtos;

namespace Application.QueryRepositories
{
    public interface IProductQueryRepository
    {
        Task<ProductDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<ProductDto>> GetAllTableAsync();
        Task<IEnumerable<ProductDto>> GetAllByBrandIdAsync(Guid brandId);
        Task<ProductDto?> GetProductWithQuantityAsync(Guid productId);
        Task<IEnumerable<ProductDto>> GetProductsWithQuantities(Guid brandId, Guid? warehouseId = null);
        Task<IEnumerable<ProductStockByWarehouseDto>> GetProductStockByWarehouseAsync(Guid productId);
        Task<IEnumerable<ProductStockByWarehouseDto>> GetAllProductStockByWarehouseAsync(Guid brandId);
        Task<IEnumerable<LowStockProductDto>> GetLowStockProductsAsync(Guid brandId, int threshold = 10);
    }
}