using Domain.Entities.Products;
using Domain.Repositories.Commands.Base;

namespace Domain.Repositories.Commands
{
    public interface IWarehouseCommandRepository : ICommandRepository<Warehouse>
    {
        Task<IEnumerable<Guid>> GetLinkedBranchIdsAsync(Guid warehouseId);
        Task ReplaceBranchLinksAsync(Guid warehouseId, IEnumerable<Guid> branchIds, Guid brandId);
    }
}
