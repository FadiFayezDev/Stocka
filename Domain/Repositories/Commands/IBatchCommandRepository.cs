using Domain.Entities.Products;
using Domain.Repositories.Commands.Base;

namespace Domain.Repositories.Commands
{
    public interface IBatchCommandRepository : ICommandRepository<Batch>
    {
        Task<IReadOnlyList<Batch>> GetAvailableBatchesForProductAsync(Guid productId, Guid brandId);
    }
}
