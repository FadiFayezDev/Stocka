using Identity.Application.Common.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Identity.Infrastructure;

internal sealed class IdentityDbConnection : IIdentityDbConnection, IDisposable
{
    public IdentityDbConnection(string connectionString)
    {
        Connection = new SqlConnection(connectionString);
    }

    public IDbConnection Connection { get; }

    public void Dispose()
    {
        Connection.Dispose();
    }
}
