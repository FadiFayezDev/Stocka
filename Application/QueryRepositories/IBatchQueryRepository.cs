using Application.Dtos.Accounting;
using Application.Dtos.Products;
using Domain.Entities.Products;

namespace Application.QueryRepositories
{
    public interface IBatchQueryRepository
    {
        Task<BatchDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<BatchDto>> GetAllTableAsync();
        Task<IEnumerable<BatchDto>> GetAllByBrandIdAsync(Guid brandId);
    }
}
