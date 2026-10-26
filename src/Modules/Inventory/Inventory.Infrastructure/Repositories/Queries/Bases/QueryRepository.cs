using System.Data;

namespace Inventory.Infrastructure.Repositories.Queries.Bases
{
    public abstract class QueryRepository
    {
        protected readonly IDbConnection _connection;

        protected const string TableWarehouses = "Inventory.Warehouse";
        protected const string TableBatches = "Inventory.Batches";
        protected const string TableWarehouseBatches = "Inventory.WarehouseBatches";
        protected const string TableStockMovements = "Inventory.StockMovements";

        protected QueryRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        protected IDbConnection GetConnection() => _connection;
    }
}
