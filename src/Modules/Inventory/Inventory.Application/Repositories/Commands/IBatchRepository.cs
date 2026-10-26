using BuildingBlocks.Application.Common.Interfaces;
using Inventory.Domain.Entities;
using Inventory.Primitives.Identifiers;

namespace Inventory.Application.Repositories.Commands
{
    public interface IBatchRepository : IRepository<Batch, BatchId>
    {
    }
}
