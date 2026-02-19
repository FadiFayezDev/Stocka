using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Infrastructure.Repositories.Queries.Base
{
    public abstract class QueryRepository
    {
        protected readonly IDbConnection _connection;

        // Table Names (snake_case for PostgreSQL)
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

        public QueryRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        protected IDbConnection GetConnection() => _connection;
    }
}
