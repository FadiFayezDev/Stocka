using System.Data;

namespace Identity.Application.Common.Interfaces;

public interface IIdentityDbConnection
{
    IDbConnection Connection { get; }
}
