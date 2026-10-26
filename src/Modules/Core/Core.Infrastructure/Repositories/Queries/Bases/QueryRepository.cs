using System.Data;

namespace Core.Infrastructure.Repositories.Queries.Bases
{
    public abstract class QueryRepository
    {
        protected readonly IDbConnection _connection;

        protected const string TableBrands = "Core.Brands";

        protected QueryRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        protected IDbConnection GetConnection() => _connection;
    }
}
