using Domain.Entities.Products;
using Domain.Repositories.Commands.Base;

namespace Domain.Repositories.Commands
{
    public interface IWarehouseBatchCommandRepository : ICommandRepository<WarehouseBatch>
    {
    }
}
