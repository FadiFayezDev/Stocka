using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Catalog.Application.Common.Interfaces
{
    public interface ICatalogDbConnection
    {
        IDbConnection Connection { get; }
    }
}
