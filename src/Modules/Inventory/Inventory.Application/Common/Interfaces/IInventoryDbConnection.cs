using System.Data;

namespace Inventory.Application.Common.Interfaces;

public interface IInventoryDbConnection
{
    IDbConnection Connection { get; }
}
