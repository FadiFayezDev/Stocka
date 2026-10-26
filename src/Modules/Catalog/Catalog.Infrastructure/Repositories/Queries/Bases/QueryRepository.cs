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

        protected const string TableProducts = "Products";
        protected const string TableCategories = "Categories";

        protected QueryRepository(IDbConnection connection /*, ICurrentUserContext? userContext = null*/ )
        {
            _connection = connection;
            //_userContext = userContext;
        }

        protected IDbConnection GetConnection() => _connection;
        //protected bool ApplyBranchScope => _userContext is { CanAccessAllBranches: false, ActiveBranchId: not null };
        //protected Guid? ActiveBranchId => _userContext?.ActiveBranchId;
        //protected Guid? ActiveBrandId => _userContext?.ActiveBrandId;
    }

}
