using Identity.Application.Common.Interfaces;
using System.Data;

namespace Identity.Infrastructure.Repositories.Queries.Bases
{
    /// <summary>
    /// Read-side (CQRS) query repository base over raw SQL (Dapper).
    /// <para>
    /// Each query repository receives a short-lived <see cref="IDbConnection"/>
    /// (registered per-request in <c>IdentityModuleRegistration</c>).
    /// Dapper opens a closed connection on execution and closes it when it opened it —
    /// so implementations never need to manage connection lifecycle per call.
    /// </para>
    /// </summary>
    internal abstract class QueryRepository
    {
        /// <summary>Dapper-target connection. Named for readability at call sites.</summary>
        protected readonly IDbConnection Connection;

        protected QueryRepository(IIdentityDbConnection connection)
        {
            Connection = connection.Connection;
        }

        /// <summary>
        /// Escapes SQL LIKE wildcards from caller input (<c>% _ [ \</c>), keeping the
        /// contains-search safe. Callers wrap the returned value in <c>%…%</c>.
        /// </summary>
        protected static string EscapeLike(string value) =>
            value.Replace("\\", "\\\\").Replace("%", "\\%").Replace("_", "\\_").Replace("[", "\\[");
    }
}
