namespace Inventory.Application.UseCases.StockCases
{
    /// <summary>
    /// Shared error messages for stock operations. Kept here (not in the domain) because they are
    /// application-orchestration wording; the entities enforce equivalent invariants with their own guards.
    /// </summary>
    public static class StockOperationMessages
    {
        public const string BatchNotFound = "Batch not found.";
        public const string WarehouseNotFound = "Warehouse not found.";
        public const string WarehouseInactive = "Warehouse is not active.";
        public const string BatchNotInWarehouse = "Batch has no placement in this warehouse.";
        public const string SourceDestinationSame = "Source and destination warehouses must differ.";
    }
}
