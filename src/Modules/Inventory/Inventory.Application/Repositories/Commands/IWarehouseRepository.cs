using BuildingBlocks.Application.Common.Interfaces;
using Inventory.Domain.Entities;
using Inventory.Primitives.Identifiers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Application.Repositories.Commands
{
    public interface IWarehouseRepository : IRepository<Warehouse, WarehouseId>
    {
    }
}
