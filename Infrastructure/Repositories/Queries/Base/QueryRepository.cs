using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Infrastructure.Repositories.Queries.Base
{
    public abstract class QueryRepository
    {
        protected readonly IDbConnection _connection;

        public QueryRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        protected IDbConnection GetConnection() => _connection;
    }
}
