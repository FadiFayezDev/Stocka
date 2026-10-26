namespace Inventory.Contracts.DTOs
{
    /// <summary>Availability detail rows for cross-module distribution queries.</summary>
    public record StockAvailabilityRow(Guid WarehouseId, int Quantity);

    /// <summary>
    /// Cross-module stock availability answer.
    /// TotalQuantity = current units in warehouses (sellable stock).
    /// UnallocatedQuantity = units in batches not yet placed into any warehouse.
    /// </summary>
    public record StockAvailabilityResponse(
        Guid ProductId,
        Guid? WarehouseId,
        int TotalQuantity,
        int UnallocatedQuantity,
        IReadOnlyList<StockAvailabilityRow> Distribution
    );

    /// <summary>Warehouse brief for cross-module consumers (e.g., destination picklists).</summary>
    public record InventoryWarehouseDto(
        Guid Id,
        string Name,
        string Location,
        bool IsActive
    );
}
