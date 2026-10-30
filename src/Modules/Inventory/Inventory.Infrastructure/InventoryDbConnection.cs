using Inventory.Application.Common.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Inventory.Infrastructure;

internal sealed class InventoryDbConnection : IInventoryDbConnection, IDisposable
{
    public InventoryDbConnection(string connectionString)
    {
        Connection = new SqlConnection(connectionString);
    }

    public IDbConnection Connection { get; }

    public void Dispose()
    {
        Connection.Dispose();
    }
}
