using Application.Common.Interfaces;
using System.Data;

namespace Infrastructure.Repositories.Queries.Base
{
    public abstract class QueryRepository
    {
        protected readonly IDbConnection _connection;
        protected readonly ICurrentUserContext? _userContext;

        protected const string TableAccounts = "accounts";
        protected const string TableBatches = "batches";
        protected const string TableBrands = "brands";
        protected const string TableBranches = "branches";
        protected const string TableBrandMemberships = "brand_memberships";
        protected const string TableCustomers = "customers";
        protected const string TableEmployees = "employees";
        protected const string TableExpenses = "expenses";
        protected const string TableExpenseCategories = "expense_categories";
        protected const string TableJournalEntries = "journal_entries";
        protected const string TableJournalEntryLines = "journal_entry_lines";
        protected const string TableOrderItems = "order_items";
        protected const string TableOrders = "orders";
        protected const string TableProducts = "products";
        protected const string TableProductCategories = "product_categories";
        protected const string TablePurchases = "purchases";
        protected const string TablePurchaseItems = "purchase_items";
        protected const string TableStockMovements = "stock_movements";
        protected const string TableSuppliers = "suppliers";
        protected const string TableWarehouses = "warehouses";
        protected const string TableWarehouseBatches = "warehouse_batches";

        protected QueryRepository(IDbConnection connection, ICurrentUserContext? userContext = null)
        {
            _connection = connection;
            _userContext = userContext;
        }

        protected IDbConnection GetConnection() => _connection;
        protected bool ApplyBranchScope => _userContext is { CanAccessAllBranches: false, ActiveBranchId: not null };
        protected Guid? ActiveBranchId => _userContext?.ActiveBranchId;
        protected Guid? ActiveBrandId => _userContext?.ActiveBrandId;
    }
}