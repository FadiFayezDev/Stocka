using Domain.Entities.Products;
using Domain.Repositories.Commands.Base;
using System.Collections.Generic;

namespace Domain.Repositories.Commands
{
    public interface IWarehouseBatchCommandRepository : ICommandRepository<WarehouseBatch>
    {
        Task<WarehouseBatch?> GetByWarehouseAndBatchAsync(Guid warehouseId, Guid batchId);
        Task<IReadOnlyList<WarehouseBatch>> GetByBatchAsync(Guid batchId);
    }
}
