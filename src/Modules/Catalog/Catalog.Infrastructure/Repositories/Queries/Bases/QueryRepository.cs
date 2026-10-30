using Catalog.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Catalog.Application.Repositories.Queries.Bases
{
    public abstract class QueryRepository
    {
        protected readonly IDbConnection _connection;
        //protected readonly ICurrentUserContext? _userContext;
        private const string Schema = "Catalog";
        protected const string TableProducts = $"{Schema}.Products";
        protected const string TableCategories = $"{Schema}.Categories";

        protected QueryRepository(ICatalogDbConnection connection /*, ICurrentUserContext? userContext = null*/ )
        {
            _connection = connection.Connection;
            //_userContext = userContext;
        }

        protected IDbConnection GetConnection() => _connection;
        //protected bool ApplyBranchScope => _userContext is { CanAccessAllBranches: false, ActiveBranchId: not null };
        //protected Guid? ActiveBranchId => _userContext?.ActiveBranchId;
        //protected Guid? ActiveBrandId => _userContext?.ActiveBrandId;
    }

}
