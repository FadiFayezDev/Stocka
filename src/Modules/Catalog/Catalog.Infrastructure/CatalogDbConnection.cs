using Catalog.Application.Common.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Catalog.Infrastructure;

internal sealed class CatalogDbConnection : ICatalogDbConnection, IDisposable
{
    public CatalogDbConnection(string connectionString)
    {
        Connection = new SqlConnection(connectionString);
    }

    public IDbConnection Connection { get; }

    public void Dispose()
    {
        Connection.Dispose();
    }
}
