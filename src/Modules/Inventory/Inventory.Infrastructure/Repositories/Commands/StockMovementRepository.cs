using Inventory.Application.Repositories.Commands;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Contexts;
using Inventory.Infrastructure.Repositories.Commands.Bases;
using Inventory.Primitives.Identifiers;

namespace Inventory.Infrastructure.Repositories.Commands
{
    internal class StockMovementRepository : Repository<StockMovement, StockMovementId>, IStockMovementRepository
    {
        public StockMovementRepository(InventoryDbContext context) : base(context)
        {
        }
    }
}
