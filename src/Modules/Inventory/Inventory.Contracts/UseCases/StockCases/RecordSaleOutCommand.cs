using Inventory.Contracts.DTOs.StockMovement;

namespace Inventory.Contracts.UseCases.StockCases;

/// <summary>
/// Issues stock out of a warehouse against a sale/order.
/// Effects (single transaction): WarehouseBatch.Quantity -= quantity, SaleOut movement with
/// Order reference. Batch-level allocations are NOT touched (the units were already allocated).
/// Sale-issued units are consumed, not returned into the batch's unallocated stock.
/// </summary>
public record RecordSaleOutCommand
{
    /// <summary>The batch the sold units come from.</summary>
    public Guid BatchId { get; init; }

    /// <summary>The warehouse the units leave.</summary>
    public Guid WarehouseId { get; init; }

    /// <summary>Number of units sold; the placement must hold at least this quantity.</summary>
    public int Quantity { get; init; }

    /// <summary>Optional soft reference to the Sales order document (ReferenceType = Order).</summary>
    public Guid? OrderReferenceId { get; init; }

    /// <summary>Optional audit note stored on the movement.</summary>
    public string? Reason { get; init; }
}